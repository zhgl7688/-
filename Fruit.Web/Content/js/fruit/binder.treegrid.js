(function(){
    var oldAcceptChanges = $.fn.treegrid.methods.acceptChanges || $.fn.datagrid.methods.acceptChanges;
    var oldMethod = $.fn.treegrid.methods;
    $.extend($.fn.treegrid.methods, {
        acceptChanges: function (jq) {
            oldAcceptChanges(jq);
            jq.parent().find('[node-id]').each(function () {
                jq.treegrid('setRowState', { id: $.attr(this, 'node-id'), state: 0 });
            });
        },
        getRowState: function (jq, id) {
            var data = oldMethod.find(jq, id);
            return data && data._row_state || 0;
        },
        setRowState: function (jq, param) {
            var data = oldMethod.find(jq, param.id);
            if (data) {
                var row = $('.datagrid-row[node-id=' + data._id + ']');
                if (row.length == 0) {
                    console.warn('row loss where ' + data._id);
                    return;
                }
                row.removeClass('datagrid-row-new').removeClass('datagrid-row-change').removeClass('datagrid-row-delete')
                switch (param.rowstate) {
                    case 1:
                        row.addClass('datagrid-row-new');
                        break;
                    case 2:
                        row.addClass('datagrid-row-change');
                        break;
                    case 3:
                        row.addClass('datagrid-row-delete');
                        break;
                }
                data._row_state = param.rowstate;
            }
        },
        getChangeRows: function (jq) {
            var data = oldMethod.getData(jq);
            var rows = [];

            function treeToRows(rows, data) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i]._row_state) {
                        var row = $.extend({}, data[i]);
                        row.children = undefined;
                        rows.push(row);
                    }
                    if (data[i].children && data[i].children.length) {
                        treeToRows(rows, data[i].children);
                    }
                }
            }

            treeToRows(rows, data);

            return rows;
        }
    });
}());

fruit.databind.default.binder.treegrid = function (databind, target, options) {
    var b = this;
    this.db = databind;
    this.jq = $(target);
    this.options = fruit.parseOptions(options, target, databind.options, 'treegrid', 'loadFilter', 'url', 'saveUrl', 'idField', 'treeField', 'iconField', 'parentField', 'parentNameField', 'fixSize');
    this.options = $.extend({idField:'Id', treeField:'Name', edit_id: null}, this.options);

    this.fixSize = function () {
        var iw = $(window).innerWidth();
        var ih = $(window).innerHeight();
        if (b.options.fixSize.h == 'auto') {
            var topHeight = 0;
            $('.z-toolbar, #condition, #master,#tt>.tabs-header').each(function () {
                //console.log($(this).marginHeight(true));
                topHeight += $(this).marginHeight(true);
            });
            //console.log('window height: ' + ih + ', other height: ' + topHeight );
            var h = ih - topHeight - 4;
            //console.log('fixSize with auto height', h);
            b.jq.treegrid('resize', { width: iw - b.options.fixSize.w, height: h });
        } else {
            b.jq.treegrid('resize', { width: iw - b.options.fixSize.w, height: ih - b.options.fixSize.h });
        }
    }

    this.loadData = function (data) {
        if (!$.isArray(data)) {
            if (typeof (b.options.treegrid) == 'string') {
                b.loadData(data[b.options.treegrid]);
            }
            return;
        }
        b.jq.treegrid('loadData', data);
        return false;
    }

    this.getData = function () { return b.jq.treegrid('getData'); };

    this.refresh = function () {
        b.jq.treegrid('reload');
    }

    this.getSelected = function () {
        return b.jq.treegrid('getSelected');
    }

    this.getSelections = function () {
        return b.jq.treegrid('getSelections');
    };

    this.getNewChildId = function (parent) {
        var data = b.jq.treegrid('getData');
        var pid = parent[b.options.idField];
        var i = 1;
        for (; i < 100; i++) {
            var id = pid + (i > 9 ? i : ('0' + i));
            if(fruit.tree.findNodeData(data, id) == null){
                return id;
            }
        }
    }

    this.select = function (id) {
        this.jq.treegrid('select', id);
    }

    this.add = function () {
        var sel = b.getSelected();
        if (sel == null) {
            sel = b.jq.treegrid('getData')[0];
        }
        var node = {_row_state:fruit.STATE_NEW};
        node._id = node[b.options.idField] = b.getNewChildId(sel);
        node._parentId = sel[b.options.idField];
        node[b.options.treeField] = '';
        node[b.options.parentField] = '';
        if (b.options.parentNameField) {
            node[b.options.parentNameField] = sel[b.options.treeField];
        }
        console.log('append', node);
        b.jq.treegrid('append', { parent: sel[b.options.idField], data: [node] });
        node[b.options.parentField] = sel[b.options.idField];

        b.select(node._id);
        b.edit();
    }

    this.edit = function () {
        var row = b.getSelected();
        if (row) {
            if (b.options.edit_id) {
                b.jq.treegrid('endEdit', b.options.edit_id);
                b.options.edit_id = null;
            }

            var opt = b.jq.treegrid('options');
            var pcols = $.grep(opt.columns[0], function (c) { return c.field == opt.parentField && c.editor && (c.editor == 'combotree' || c.editor.type == 'combotree'); });
            if (pcols.length > 0) {
                var nodeData = b.jq.treegrid('getSelected');
                var treeData = b.jq.treegrid('getData');
                var treeDataString = JSON.stringify(treeData);
                var idRegex = /_id/g;
                if (opt.idField) {
                    idRegex = new RegExp(opt.idField, 'g');
                }
                if (treeData.length && treeData[0][opt.treeField + '_RefText'] != undefined) {
                    treeData = JSON.parse(treeDataString.replace(idRegex, "id").replace(new RegExp(opt.treeField + '_RefText', "g"), "text"));
                } else {
                    treeData = JSON.parse(treeDataString.replace(idRegex, "id").replace(new RegExp(opt.treeField, "g"), "text"));
                }
                fruit.tree.remove(treeData, function (n) { return n.id == nodeData[opt.idField]; });
                treeData.unshift({ "id": 0, "text": "" });
                pcols[0].editor = {
                    type: 'combotree', options: {
                        data: treeData, onChange: function (newValue, oldValue) {
                            var parentNode = fruit.tree.findNodeData(treeData, newValue, opt.idField);
                            if (parentNode && opt.parentNameField) {
                                row[opt.parentNameField] = parentNode.text;
                            }
                            else if (parentNode && parentNode[opt.treeField + '_RefText']) {
                                row[opt.parentField + '_RefText'] = parentNode[opt.treeField + '_RefText'];
                            }
                        }
                    }
                };
            }

            b.options.edit_id = row[b.options.idField];
            b.jq.treegrid('beginEdit', b.options.edit_id);
        }
    }

    this.getEditingRows = function () {
        var rows = [];
        $.each(b.jq.treegrid('getPanel').find('.datagrid-row-editing[node-id]'), function () {
            var rowId = $(this).attr('node-id');
            if (rows.indexOf(rowId) == -1) {
                rows.push(rowId);
            }
        });
        return rows;
    }

    this.endEdit = function () {
        var editRows = b.getEditingRows();
        $.each(editRows, function () {
            console.log('endEdit','' + this);
            b.jq.treegrid('endEdit', '' + this);
        });
    }

    this.delete = function () {
        var node = b.getSelected();
        if (node) {
            var rowState = b.jq.treegrid('getRowState', node[b.options.idField]);
            if (rowState == fruit.STATE_NEW) {
                b.jq.treegrid('remove', node[b.options.idField]);
                return;
            }
            b.jq.treegrid('setRowState', { id: node[b.options.idField], rowstate: fruit.STATE_DELETED });
        }
    }

    this.save = function () {
        b.endEdit();
        if (b.jq.treegrid('getPanel').find('.validatebox-invalid').length) {
            fruit.message('error', '请更正验证错误！');
            return;
        }
        var rows = b.jq.treegrid('getChangeRows');
        if (rows.length > 0) {
            b.jq.showLoading('正在保存数据…');
            fruit.ajax({
                url: b.options.saveUrl,
                data: JSON.stringify(rows),
                success: function (data) {
                    b.jq.hideLoading();
                    b.jq.treegrid('acceptChanges');
                    b.db.invoke('onSave');
                }
            });
        } else {
            b.db.invoke('onSave');
        }
    }

    this.findItem = function (key) {
        var data = this.getData();
        return fruit.tree.findNodeData(data, key, b.options.idField);
    }

    var opts = {
        onDblClickRow: function (row) {
            b.jq.treegrid('unselectAll');
            b.jq.treegrid('select', row[b.options.idField]);
            b.db.invoke('edit');
            //b.edit();
        },
        onSelect: function (row) {
            if (b.options.edit_id) {
                b.jq.treegrid('endEdit', b.options.edit_id);
                b.options.edit_id = null;
            }
        },
        onAfterEdit: function (row, changes) {
            var rowstate = row && row._row_state || 0;
            if (rowstate != fruit.STATE_NEW) {
                b.jq.treegrid('setRowState', { id: row._id, rowstate: fruit.STATE_CHANGED });
            }
        }
    };
    this.jq.treegrid(opts);
    if (b.options.fixSize) {
        b.fixSize();
        $(window).resize(function () {
            b.fixSize();
        });
    }
}