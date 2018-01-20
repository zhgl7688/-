fruit.formatter = {};
fruit.formatter.Enable = function (value, row) { return '<center><img src="/Content/images/' + ((value || '').toString() == "true" || value == 1 ? "checkmark.gif" : "checknomark.gif") + '"/></center>'; };
fruit.formatter.Time = function (value, row) { return value && value.replace('T', ' '); };
fruit.formatter.Date = function (value, row) { return $.isString(value) ? value.split('T')[0] : ''; };
fruit.formatter.Money = function (value, row) { };
fruit.formatter.File = function (value, row) { return $.fn.file.methods.createFileCell(value); };

(function () {


    var oldMethod = $.fn.datagrid.methods;
    var oldAcceptChanges = $.fn.datagrid.methods.acceptChanges;
    $.extend($.fn.datagrid.methods, {
        acceptChanges: function (jq) {
            var needDelRowIndexs = [];
            jq.parent().find('[datagrid-row-index]').each(function (rowIndex) {
                if (jq.datagrid('getRowState', rowIndex) == fruit.STATE_DELETED) {
                    needDelRowIndexs.push(rowIndex);
                } else {
                    jq.datagrid('setRowState', { index: $.attr(this, 'datagrid-row-index'), state: 0 });
                }
            });

            for (var i = 0; i < needDelRowIndexs.length; i++) {
                var rowIndex = needDelRowIndexs[i] - i;
                jq.datagrid('deleteRow', rowIndex);
            }

            oldAcceptChanges(jq);
        },
        getRowState: function (jq, index) {
            var row = oldMethod.getRows(jq)[index];
            return row && row._row_state || 0;
        },
        setRowState: function (jq, param) {
            var data = oldMethod.getRows(jq)[param.index];
            if (data) {
                var row = oldMethod.getPanel(jq).find('.datagrid-row[datagrid-row-index=' + param.index + ']');
                if (row.length == 0) {
                    console.warn('row loss where index ' + param.index);
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
            var data = oldMethod.getRows(jq);
            var rows = [];

            for (var i = 0; i < data.length; i++) {
                if (data[i]._row_state) {
                    var row = $.extend({}, data[i]);
                    rows.push(row);
                }
            }

            return rows;
        }
    });



    var iconSelectorDialog = null;
    var iconSelectorCallback = function () { };

    // datagrid 编辑器扩展
    $.extend($.fn.datagrid.defaults.editors, {
        'icons': {
            init: function (container, options) {
                var input = $('<input type="text" class="datagrid-editable-input" />').appendTo(container);
                var btn = $('<input type="button" value=".." class="datagrid-editable-morebutton" />').appendTo(container).click(function () {

                    if (iconSelectorDialog == null) {
                        var jq = $('link[href$="icon.css"]');
                        if (jq.length == 0) {
                            alert('使用 icons 类型编辑器需要在当前页面引用 icon.css 样式表文件!');
                            return;
                        }
                        var rules = jq[0].sheet.cssRules;
                        iconSelectorDialog = $('<div></div>');
                        for (var i = 0; i < rules.length; i++) {
                            if (rules[i].selectorText.indexOf('.icon-') == 0 && rules[i].selectorText.indexOf('32') == -1) {
                                $('<span class="icon-selector ' + rules[i].selectorText.substr(1) + '">&nbsp;</span>').appendTo(iconSelectorDialog).click(function () {
                                    var val = $(this).attr('class').split(' ')[1];
                                    iconSelectorDialog.dialog('close');
                                    iconSelectorCallback(val);
                                });
                            }
                        }
                        iconSelectorDialog.dialog({ title: '选择图标', modal: true, closed: false, width: 400, height: 280 });
                    } else {
                        iconSelectorDialog.dialog('open');
                    }
                    iconSelectorCallback = function (val) {
                        input.val(val);
                    }
                });
                return input;
            },
            destroy: function (target) {
            },
            getValue: function (target) {
                return $(target).val();
            },
            setValue: function (target, value) {
                $(target).val(value);
            },
            resize: function (target, width) {
                $(target)._outerWidth(width - 18);
            },
        },
        'popup': {
            init: function(container, options)
            {
                var input = $('<input type="text" class="datagrid-editable-input easyui-validatebox" />').appendTo(container);
                input.popup(options);
                input.data('popup_options', options);
                return input;
            },
            destroy: function (target) {
                $(target).popup('destroy');
            },
            getValue: function (target) {
                return $(target).val();
            },
            setValue: function (target, value) {
                $(target).val(value);
                var options = $(target).data('popup_options');
                if (options.textField) {
                    var row = null;
                    var tr = $(target).closest('tr.datagrid-row');
                    if (tr.is('[node-id]')) { // treegrid
                        row = fruit.databind.root.binds.grid.findItem(tr.attr('node-id'));
                    } else if (tr.is('[datagrid-row-index]')) { // datagrid
                        row = fruit.databind.root.binds.grid.findItem(parseInt(tr.attr('datagrid-row-index')));
                    }

                    if (row && row[options.textField]) {
                        $.fn.popup.methods.setText(target, row[options.textField]);
                    }
                }
            },
            resize: function (target, width) {
                $(target).next()._outerWidth(width);
            },
        }
    });

}());


fruit.databind.default.binder.datagrid = function (databind, target, options) {
    var b = this;
    this.db = databind;
    this.jq = $(target);
    this.jq.data('binder', b);
    this.options = fruit.parseOptions(options, target, databind.options, 'datagrid', 'autoload', 'loadFilter', 'url', 'saveUrl', 'idField', 'iconField', 'fixSize', 'toolbar', 'editMode', 'holdTotal', 'preQueryParams');
    this.options = $.extend({ idField: 'Id', edit_id: -1 }, this.options);

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
            b.jq.datagrid('resize', { width: iw - b.options.fixSize.w, height: h });
        } else {
            b.jq.datagrid('resize', { width: iw - b.options.fixSize.w, height: ih - b.options.fixSize.h });
        }
    }

    this.add = function (row) {
        if (row == undefined || row instanceof $.Event) {
            row = {};
            row[b.options.idField] = '';
        }
        row._id_ = fruit.databind.newid();
        if($.isFunction(b.db.options.onBeforeAdd)){
            if (b.db.options.onBeforeAdd(row, b) === false) {
                return;
            }
        }
        var rowIndex = b.jq.datagrid('getRows').length;
        b.jq.datagrid('appendRow', row);
        b.jq.datagrid('selectRow', rowIndex);
        b.jq.datagrid('setRowState', { index: rowIndex, rowstate: fruit.STATE_NEW });
        b.edit();
    }

    this.getRows = function () {
        return b.jq.datagrid('getRows');
    }

    this.setLoadParams = function (params) {
        if (b.options.autoload === false) {
            b.options.autoload = true;
            b.jq.datagrid('options').url = b.options.url;
        }
        if (params == undefined) {
            params = {};
        }
        if (b.options.preQueryParams) {
            params = $.extend({}, params, b.options.preQueryParams);
        }
        if (b.options.holdTotal && b.jq.datagrid('options').queryParams && b.jq.datagrid('options').queryParams.total) {
            params.total = b.jq.datagrid('options').queryParams.total;
        }
        b.jq.datagrid('options').queryParams = params;
        b.jq.datagrid('load');
    }

    this.refresh = function () {
        if (b.options.autoload === false) {
            b.options.autoload = true;
            b.jq.datagrid('options').url = b.options.url;
        }
        b.jq.datagrid('reload');
    }

    this.edit = function () {
        if (b.jq.attr('readonly')) {
            return false;
        }
        var row = b.jq.datagrid('getSelected');
        var rowIndex = b.jq.datagrid('getRows').indexOf(row);
        if (rowIndex > -1) {
            b.jq.datagrid('beginEdit', rowIndex);

            var editors = b.jq.datagrid('getEditors', rowIndex);
            var row = b.jq.datagrid('getRows')[rowIndex];
            $.each(editors, function () {
                if (this.type == 'popup' && row) {
                    var textField = $(this.target).data().popup.options.textField;
                    if (textField && row[textField]) {
                        $.fn.popup.methods.setText(this.target, row[textField]);
                    }
                }
            });

            b.options.edit_id = rowIndex;
        }
    };

    //this.endEdit = function () {
    //    if (b.options.edit_id != -1) {
    //        b.jq.datagrid('endEdit', b.options.edit_id);
    //    }
    //};

    this.getEditingRows = function () {
        var rows = [];
        $.each(b.jq.datagrid('getPanel').find('.datagrid-row-editing[datagrid-row-index]'), function () {
            var rowIndex = parseInt($(this).attr('datagrid-row-index'));
            if (rows.indexOf(rowIndex) == -1) {
                rows.push(rowIndex);
            }
        });
        return rows;
    }

    this.endEdit = function () {
        var editRows = b.getEditingRows();
        for (var rowIndex = 0; rowIndex < editRows.length; rowIndex++) {
            var idx = editRows[rowIndex];
            b.jq.datagrid('endEdit', editRows[rowIndex]);
        }
    };

    this.getSelected = function () {
        return b.jq.datagrid('getSelected');
    };

    this.getSelections = function () {
        return b.jq.datagrid('getSelections');
    };

    this.joinSelectedColumn = function (column) {
        var array = [];
        var rows = b.getSelections();
        $.each(rows, function () {
            array.push(this[column]);
        });
        return array.join();
    }

    this.delete = function () {
        var rows = b.jq.datagrid('getSelections');
        var removeRows = [];
        $.each(rows, function () {
            var row = this;
            var rowIndex = b.jq.datagrid('getRows').indexOf(row);
            if (rowIndex > -1) {
                if (b.jq.datagrid('getRowState', rowIndex) == fruit.STATE_NEW) {
                    removeRows.push(rowIndex);
                } else {
                    b.jq.datagrid('setRowState', { index: rowIndex, rowstate: fruit.STATE_DELETED });
                }
            }
        })
        $.each(removeRows, function (i) {
            b.jq.datagrid('deleteRow', this - i);
        })
    };

    this.save = function () {
        b.endEdit();
        if (!b.jq.datagrid('validateRow', b.options.edit_id)) {
            fruit.message('error', '请更正验证错误！');
            return;
        }
        var rows = b.jq.datagrid('getChangeRows');
        if (rows.length > 0) {
            //console.log(rows);
            b.jq.showLoading('正在保存数据…');
            fruit.ajax({
                url: b.options.saveUrl || b.options.url,
                data: JSON.stringify(rows),
                success: function (data) {
                    b.jq.hideLoading();
                    if ($.isArray(data)) {
                        b.jq.datagrid('loadData', data);
                    }
                    b.jq.datagrid('acceptChanges');
                    b.db.invoke('onSave');
                }
            });
        } else {
            b.db.invoke('onSave');
        }
    }

    this.onEditing = function (rowIndex, row, field, newValue) {
        if (b.jq.find('th[field="' + field + '"]').data('expression')) {
            return;
        }
        b.calcColumn(rowIndex);
        b.updateSumRow();
    };

    this.calcColumn = function (index) {
        var editors = this.jq.datagrid('getEditors', index);
        var row = this.jq.datagrid('getRows')[index];

        var replaceFunc = function ($0, $1) {
            var editor = editors.find('field', $1);
            if (editor) {
                return editor.actions.getValue(editor.target);
            } else {
                return row[$1];
            }
            return $0;
        };

        b.jq.find('th[data-expression]').each(function () {
            var expression = $(this).data('expression');
            var field = $(this).attr('field');
            var code = expression.replace(/\{([^\}]*)}/g, replaceFunc);
            var result = null;
            try{
                eval('result = ' + code);
                var editor = editors.find('field', field);
                if (editor) {
                    editor.actions.setValue(editor.target, result);
                } else {
                    row[field] = result;
                    b.setCellText(index, field, result);
                    //$.each(b.jq.datagrid('getEditors', index), function () {
                    //    if (this.field) {
                    //        row[this.field] = this.actions.getValue(this.target);
                    //    }
                    //});
                    //b.jq.datagrid('updateRow', { index: index, row: row });
                    //b.jq.datagrid('endEdit', index);
                }
            }
            catch (e)
            {
            }
        });
    };

    b.setCellText = function (rowIndex, field, text) {
        var cell = b.jq.datagrid('getPanel').find('tr[datagrid-row-index=' + rowIndex + ']>td[field=' + field + ']>.datagrid-cell');
        if (cell) {
            cell.text(text);
        }
    };

    b.updateSumRow = function () {
        var rows = b.getRows();
        b.setSumRow(rows);
    };

    b.setSumRow = function (data) {
        var sumFields = b.jq.find('th[field][sumfield="true"]');
        if (sumFields.length > 0) {
            var sumRow = {};
            $.each(data, function () {
                var row = this;
                sumFields.each(function () {
                    var field = $(this).attr('field');
                    try {
                        if (row[field] && $.isNumeric(row[field])) {
                            if (!sumRow[field]) {
                                sumRow[field] = 0;
                            }
                            sumRow[field] += parseFloat(row[field]);
                        }
                    } catch (e) { }
                })
            });
            b.jq.datagrid('reloadFooter', [sumRow]);
        }
    };

    b.findItem = function (index) {
        return b.getRows()[index];
    }

    var opts = {
        onSelect: function (index, row) {
            if (b.options.edit_id > -1) {
                if (b.jq.datagrid('validateRow', b.options.edit_id)) {
                    b.jq.datagrid('endEdit', b.options.edit_id);
                    b.options.edit_id = -1;
                } else {
                    return;
                }
            }
            if (b.options.editMode == 'select') {
                b.edit();
            }
        },
        onBeginEdit: function (index, row) {
            var editors = b.jq.datagrid('getEditors', index);
            //var row = b.jq.datagrid('getRows')[index];
            var gridOpts = b.jq.datagrid('options');
            $.each(editors, function () {
                var editor = this;
                var editorOpt = gridOpts.columns[0].find(function (e) { return e.field == editor.field });
                if (editorOpt && editorOpt.editor && editorOpt.editor.options && editorOpt.editor.options.modifiable === false) {
                    editorOpt.editor.options.disabled = row._row_state != fruit.STATE_NEW;
                    editor.target.textbox(editorOpt.editor.options.disabled ? 'disable' : 'enable');
                }
                if (editor.type == 'numberbox') {
                    $(editor.target).numberbox({
                        onChange: function (newValue, oldValue) {
                            b.onEditing(index, row, editor.field, newValue);
                        }
                    });
                }
            });
        },
        onEndEdit: function (index, row, changes) {
            //console.log('onEndEdit', index, row, changes);
            if (!$.isEmptyObject(changes) && index > -1 && index < b.jq.datagrid('getRows').length && row._row_state != fruit.STATE_NEW) {
                b.jq.datagrid('setRowState', { index: index, rowstate: fruit.STATE_CHANGED });
            }
        },
        onLoadSuccess: function (data) {
            b.setSumRow(data.rows);
            if ($.isFunction(b.db.onLoadSuccess)) {
                b.db.onLoadSuccess.call(b.db, data);
            }
        }
    };
    if (b.options.url) {
        opts.url = b.options.url;
        opts.method = b.options.method || 'get';
    }
    if (b.options.holdTotal) {
        opts.onLoadSuccess = function(data){
            if (data.total) {
                b.jq.datagrid('options').queryParams.total = data.total;
            }
            if ($.isFunction(b.db.onLoadSuccess)) {
                b.db.onLoadSuccess.call(b.db, data);
            }
        }
    }
    if (b.options.preQueryParams) {
        opts.queryParams = b.options.preQueryParams;
    }
    if (b.options.toolbar) {
        opts.toolbar = b.options.toolbar;
    }
    if (b.options.autoload === false) {
        opts.url = null;
    }
    this.jq.datagrid(opts);
    if (b.options.fixSize) {
        b.fixSize();
        $(window).resize(function () {
            b.fixSize();
        });
    }
};