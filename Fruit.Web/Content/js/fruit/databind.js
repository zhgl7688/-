fruit.databind = function (element, options) {
    if (!(this instanceof fruit.databind)) {
        throw new Error("fruit.databind 只能使用 new 方式调用！");
    }
    var d = this;
    d.id = Math.random();
    d.toString = function () {
        return d.id;
    }
    d.parsed = false;
    d.jq = $(element);
    d.jq.data('fruit_databind', this);
    d.jq.attr('fruit', 'databind')
    d.options = $.extend({}, fruit.databind.default, options);

    d.binds = [];

    d.validate = function () {
        $.each(d.binds, function () {
            if (this instanceof fruit.databind.default.binder.datagrid) {
                this.endEdit();
            }
        });
        return d.jq.find('.validatebox-invalid').length == 0;
    };

    d.binderByElement = function (el) {
        return d.binds.find(function (binder) { return binder.jq[0] == el; });
    };

    d.invoke = function (method, params) {
        if ($.isFunction(d.options[method])) {
            return d.options[method].call(d, params);
        } else if ($.isFunction(d[method])) {
            return d[method](params);
        } else {
            console.log('databind.invoke');
            var hasInvoked = false;
            var result = undefined;
            $.each(d.binds, function () {
                if ($.isFunction(this[method])) {
                    hasInvoked = true;
                    result = this[method].call(this, params);
                    if (result === false) {
                        return false;
                    } else if (typeof result != 'undefined') {
                        return false;
                    }
                }
            });
            //if (!hasInvoked && $.isFunction(d[method])) {
            //    return d[method].call(d, params);
            //}
            //if (d !== fruit.databind.root) {
            //    // 向父级发布事件

            //    return;
            //}
            if (!hasInvoked) {
                alert('未找到可用的 ' + method + ' 方法绑定');
            }
            console.log('databind.invoke result', result);
            return result;
        }
    };

    d.load = function (opt) {
        if (typeof opt == 'string') {
            opt = { url: opt };
        }
        opt = $.extend({}, { method: 'GET' }, opt);
        opt.success = function (data) {
            d.loadData(data);
        };
        fruit.ajax(opt);
    }

    d.loadData = function (data) {
        d.options.data = data;
        $.each(d.binds, function () {
            if ($.isFunction(this.bindData)) {
                this.bindData.call(this, data);
            }
        });
    }

    d.clearData = function (name) {
        $.each(d.binds, function () {
            if ($.isFunction(this.clearValue)) {
                this.clearValue.call(this, name);
            }
        });
    }

    d.getData = function (name) {
        var obj = {};
        $.each(d.binds, function () {
            if ($.isFunction(this.getData)) {
                this.getData(obj);
            }
        });
        if ($.isString(name)) {
            var names = name.split('.');
            for (var i = 0; i < names.length; i++) {
                if (typeof obj[names[i]] == 'undefined') {
                    return undefined;
                }
                obj = obj[names[i]];
            }
            return obj;
        }
        return obj;
    }

    d.init = function () {
        d.jq.find('[data-bind]').each(function () {
            var bind = $(this).data('bind');
            var bitems = bind.split(',');
            if (bitems[0].split(':').length == 1) {
                if (bitems.length == 1) {
                    bind += ':true';
                } else {
                    bitems[0] = bitems[0] + ':true';
                    bind = bitems.join(',');
                }
            }
            eval('options = {' + bind + '};');
            for (var key in options) {
                if ($.isFunction(d.options.binder[key])) {
                    var inst = new d.options.binder[key](d, this, options);
                    d.binds.push(inst);
                    if (this.id) {
                        d.binds[this.id] = inst;
                    }
                }
            }
        });

        //console.log('parser test', d.id, element != document.body, d.parsed);
        if (element != document.body && d.parsed !== true) {
            d.parsed = true;
            $.parser.onComplete = function () {
                //console.log(d.id, 'parser onComplete');
                d.finshInit.call(d);
            }
            $.parser.parse(element);
            return;
        }

        d.finshInit();
    },

   d.finshInit = function () {
        if (d.binds.grid instanceof fruit.databind.default.binder.datagrid || $('table[data-bind^="datagrid"]').length) {
            $(element).on('dblclick', '.datagrid-row', function () {
                var dg = $(this).closest('.datagrid-view').find('>table[data-bind="datagrid"]');
                dg.datagrid('clearSelections').datagrid('selectRow', parseInt($(this).attr('datagrid-row-index')));
                d.invoke('edit');
            });
        }
        // group filter support
        $(element).on('click', '.group-filter .gf-item', function () {
            //console.log(this, $(this).data('name'));
            $(this).siblings().removeClass('selected').end().addClass('selected');
            $(this).siblings('input').val($(this).data('name'));
            d.invoke('searchClick');
        });


        // 修正 combobox 固定宽度问题
        $('.val>.easyui-combobox').combobox('resize', 'auto');

        d.options.onInit.call(d);
    },

    d.uninit = function () {
        d.parsed = false;
        if (d.binds.grid || $('table[data-bind^="datagrid"]').length) {
            $(element).off('dblclick', '.datagrid-row');
        }
        for (var key in d.binds) {
            var inst = d.binds[key];
            if (inst && typeof inst.uninit == 'function') {
                inst.uninit();
            }
        }
    }

    d.download = function (e) {
        //var target = $(e.currentTarget);
        var grid = null;
        $.each(d.binds, function () {
            if (this instanceof fruit.databind.default.binder.datagrid) {
                grid = this;
                return false;
            }
        });
        if (grid == null) {
            return;
        }

        var gridOpt = grid.jq.data('datagrid').options;
        var data = {
            title: document.title,
            url: gridOpt.url,
            columns: gridOpt.columns,
            queryParams: gridOpt.queryParams
        };

        
        var downloadHelper = $('<iframe style="display:none;" id="downloadHelper"></iframe>').appendTo('body')[0];
        var doc = downloadHelper.contentWindow.document;
        if (doc) {
            doc.open();
            doc.write('')//微软为doc.clear();
            doc.writeln("<html><body><form id='downloadForm' name='downloadForm' method='post' action='/Home/Download'>");
            doc.writeln("<input type='hidden' name='data' value='" + JSON.stringify(data) + "'>");
            doc.writeln('<\/form><\/body><\/html>');
            doc.close();
            var form = doc.forms[0];
            if (form) {
                form.submit();
            }
        }
    }

    d.printClick = function (e) {
        var report = $(event.target).closest('[report]').attr('report');
        var report_params = $(event.target).closest('[report]').attr('report_params').split(',');
        if (report_params.length == 1 && report_params[0].indexOf('#') == 0) {

            var grid = fruit.databind.root.binds[report_params[0].substr(1)];
            if (!grid) {
                return;
            }

            var apiUrl = grid.options.url;
            var apiArgs = grid.jq.datagrid('options').queryParams;
            var data = $.extend({}, apiArgs, { _report_: 1 });
            fruit.ajax({
                url: apiUrl,
                method: 'GET',
                data: data,
                success: function (data) {
                    parent.showDocTab('icon-printer', '打印', '/Report.aspx?ReportFile=' + report + '&condition=' + encodeURIComponent(data));
                }
            });

            return;
        }
        var formData = d.getData('form');
        var data = [];
        for (var i = 0; i < report_params.length; i++) {
            var name = report_params[i];
            data.push(name + '=' + encodeURIComponent(formData[name]));
        }
        
        parent.showDocTab('icon-printer', '打印', '/Report.aspx?ReportFile=' + report + '&' + data.join('&'));
    }

    d.getBindByType = function (type) {
        var bind = null;
        $.each(d.binds, function () {
            if (this instanceof type) {
                bind = this;
                return false;
            }
        });
        return bind;
    }

    d.popupSearch = function () {
        var data = d.getData('form');
        var list = d.getBindByType(fruit.databind.default.binder.datagrid);
        list.setLoadParams(data);
    }

    d.init();
}

fruit.databind.default = {
    onInit: $.noop,
    onSave: $.noop
}

fruit.databind.get = function(el)
{
    return $(el).closest('[fruit="databind"]').data('fruit_databind');
}

fruit.databind.default.binder = {};

fruit.databind.from = function (element) {
    return $(element).data('fruit_databind') || new fruit.databind(element);
}

fruit.popupSelected = function (row, valueFields, returnFields) {
    var form = {};
    $.each(returnFields, function (index) {
        if (this.length) {
            form[this] = row[valueFields[index]];
        }
    });
    fruit.databind.root.loadData({ form: form });
}

fruit.popupSelecteds = function (target, values, texts) {
    var form = {};
    
    var bindOpt = target.data('bind');
    eval('bindOpt = {' + bindOpt + '};');
    
    form[bindOpt.value.split('.')[1]] = values.join();
    form[bindOpt.text.split('.')[1]] = texts.join();

    console.log('popupSelecteds', form);

    fruit.databind.root.loadData({ form: form });
}

fruit.popupColumnSelected = function (row, valueFields, returnFields, textField, text) {
    $.each(fruit.databind.root.binds, function () {
        if (this instanceof fruit.databind.default.binder.datagrid) {
            var editRowIdx = this.options.edit_id;
            if (editRowIdx > -1) {
                var tblRow = this.jq.datagrid('getRows')[editRowIdx];
                var editors = this.jq.datagrid('getEditors', editRowIdx);
                if (editors && editors.length) {
                    if (textField.length) {
                        tblRow[textField] = text;
                    }
                    $.each(returnFields, function (index) {
                        if (this.length) {
                            var val = row[valueFields[index]];
                            var editor = editors.find('field', this);
                            if (editor) {
                                editor.actions.setValue(editor.target, val);
                            } else {
                                tblRow[this] = val;
                            }
                        }
                    });
                    return false;
                }
            }
        }
    });
}

fruit.popupColumnSelecteds = function (target, values, texts) {
    alert('目前不支持在子表中使用多选弹出框！')
}

fruit.databind._id_ = 1;
fruit.databind.newid = function () {
    return fruit.databind._id_++;
}

$(function () {
    fruit.databind.root = new fruit.databind(document.body);
})