$.fn.showLoading = function (text) {
    if (typeof text == 'undefined') {
        text = "正在加载数据…";
    }
    return this.each(function () {
        $('<div class="datagrid-mask-msg" style="display:block;left:50%;"></div>').text(text).insertAfter(this);
        $('<div class="datagrid-mask" style="display:block"></div>').insertAfter(this);
    });
}

$.fn.hideLoading = function () {
    this.parent().find('div.datagrid-mask,div.datagrid-mask-msg').remove();
    return this;
}

$.fn.bodyOffset = function () {
    var ele = $($(this)[0]);
    var offset = ele.offset();
    while (!!(ele = ele.offsetParent())) {
        if (ele[0] == document || ele[0] == document.body || ele[0].tagName == 'HTML') break;
        var poffset = ele.offset();
        if (poffset) {
            $.isNumeric(poffset.left) && (offset.left += poffset.left);
            $.isNumeric(poffset.top) && (offset.top += poffset.top);
        }
    }
    return offset;
}


// auto complete plugin
$(function () {

    var autocomplete = function (element, options) {
        var a = this;
        a.jq = $(element);
        a.options = fruit.parseOptions(element, options, {method:'get'}, 'url', 'method');
        a.oldValue = '';
        a.waitTime = null;
        a.panel = null;

        a.load = function () {
            var val = a.jq.val();
            if(a.oldValue == val){
                // no change;
                return;
            }
            a.oldValue = val;
            if(a.oldValue == '') {
                a.close();
                // empty;
                return;
            }
            var loadOpt = $.extend({}, {
                data: 'q=' + a.oldValue,
                success: function (data) {
                    a.options.rows = data;
                    if (a.options.rows.length > 0) {
                        a.show();
                    } else {
                        a.close();
                    }
                }
            }, a.options);
            //console.log(loadOpt);
            fruit.ajax(loadOpt);
        };

        a.autoSelect = function () {
            if (a.panelBody.find('.combobox-item-selected').length) {
                a.panelBody.find('.combobox-item-selected').trigger('click');
            } else {
                $(a.panelBody.find('.combobox-item')[0]).trigger('click');
            }
        };

        a.select = function (rowIndex) {
            a.jq.val(a.options.rows[rowIndex]);
        };

        a.show = function () {
            if (a.panel == null) {
                a.panel = $('<div style="position: absolute;"></div>').appendTo(document.body);
                a.panelBody = $('<div style="border: solid 1px #95B8E7; background-color:white; cursor:default;"></div>').appendTo(a.panel);
            }

            a.panelBody.empty();
            $.each(a.options.rows, function (i) {
                $('<div class="combobox-item"></div>').data('rowIndex', i).text(this).appendTo(a.panelBody).hover(function () {
                    $(this).addClass('combobox-item-hover');
                }, function () {
                    $(this).removeClass('combobox-item-hover');
                }).click(function () {
                    var rowIndex = $(this).data('rowIndex');
                    if (rowIndex > -1 && rowIndex < a.options.rows.length) {
                        a.select(rowIndex);
                        a.close();
                    }
                });
            });

            var offset = a.jq.bodyOffset();
            a.panel.css({ width: a.jq.outerWidth(), left: offset.left, top: offset.top }).show();
            a.options.opened = true;
        };

        a.close = function () {
            if (a.panel) {
                a.panel.hide();
            }
            a.options.opened = false;
        };

        a.jq.on('keydown', function (e) {
            if (a.options.opened) {
                var idx = a.panelBody.find('.combobox-item-selected').index();
                //console.log('idx', idx);
                switch (e.keyCode) {
                    case fruit.keyCode.ENTER:
                        a.autoSelect();
                        break;
                    case fruit.keyCode.ESC:
                        a.close();
                        break;
                    case fruit.keyCode.DOWN:
                        idx++;
                        if (idx < a.options.rows.length) {
                            a.panelBody.find('.combobox-item').removeClass('combobox-item-selected');
                            $(a.panelBody.find('.combobox-item')[idx]).addClass('combobox-item-selected');
                        }
                        break;
                    case fruit.keyCode.UP:
                        idx--;
                        if (idx > -1) {
                            a.panelBody.find('.combobox-item').removeClass('combobox-item-selected');
                            $(a.panelBody.find('.combobox-item')[idx]).addClass('combobox-item-selected');
                        }
                    default:
                        //console.log(e.keyCode);
                        break;
                }
            }
        });

        a.jq.on('keypress keyup', function () {
            clearTimeout(a.waitTime);
            a.waitTime = setTimeout(function () {
                a.load();
            }, 500);
        });

        a.jq.on('blur', function () {
            setTimeout(function () {
                a.close();
            }, 200);
        });
    }

    $.fn.autocomplete = function (options, params) {
        this.each(function () {
            var jq = $(this);
            var inst = $.data(jq, 'autocomplete');
            if (!inst) {
                inst = new autocomplete(this, options);
                $.data(jq, 'autocomplete', inst);
            }
        })
    };

    $('.easyui-autocomplete').autocomplete();
});

// picutre well plugin
$(function () {
    var pictureWall = function (options) {
        var that = this;
        this.options = $.extend({}, pictureWall.defaults, options);
        this.bg = $('<div style="position: absolute;left:0;top:0;right:0;bottom:0;z-index:9999999;background-color:rgba(0,0,0,0.5);display:none;"></div>').appendTo(top.document.body);
        this.img = $('<div style="width:100%;height:100%;background:no-repeat;background-size:contain;background-position:center center;"></div>').appendTo(this.bg);
        this.btnPrev = $('<div style="position: absolute; top:0;left:0;bottom:0;width:25%;cursor:pointer;" title="上一张"></div>').appendTo(this.bg)
            .on('click', function () {
                that.prev();
            });
        this.btnNext = $('<div style="position: absolute; top:0;right:0;bottom:0;width:25%;cursor:pointer;" title="下一张"></div>').appendTo(this.bg)
            .on('click', function () {
                that.next();
            });
        this.btnClose = $('<span class="icon-cross" style="position: absolute;display:block;top:10px;right:10px;width: 20px;height: 20px;background-color: black;background-position: center center;cursor:pointer;"></span>').appendTo(this.bg)
            .on('click', function () {
            that.close();
            });
        this.imglist = $('<div style="display: block;position: relative;top:-100px;text-align: center;margin: 0 auto;opacity:0;background-color:rgba(0,0,0,0.8);padding:10px 0;"></div>').appendTo(this.bg)
            .mouseenter(function () { $(this).css('opacity', 0.8); }).mouseleave(function () { $(this).css('opacity', 0); });
    };
    var fn = pictureWall.prototype;
    fn.close = function () {
        this.bg.fadeOut('fast');
    }
    fn.goto = function (index) {
        this.options.index = index;
        this.img.css('background-image', 'url(' + this.options.images[index] + ')');
    }
    fn.prev = function () {
        if (this.options.index > 0) {
            this.goto(this.options.index - 1);
        }
    }
    fn.next = function () {
        if (this.options.index + 1 < this.options.images.length) {
            this.goto(this.options.index + 1);
        }
    }
    fn.show = function () {
        var that = this;

        that.imglist.empty();
        $.each(that.options.images, function (idx) {
            if (idx == that.options.index) {
                that.img.css('background-image', 'url(' + this + ')');
            }
            var item = $('<div style="display:inline-block;width:80px;height:80px;background:no-repeat;background-size:cover;cursor:pointer"></div>')
                .css('background-image', 'url(' + this + ')')
                .appendTo(that.imglist)
                .on('click', function () {
                    that.goto(idx);
                });
            if (idx > 0) {
                item.css('margin-left', 10);
            }
        });

        that.bg.fadeIn('fast');
    }

    pictureWall.defaults = {
        images: [],
        index: 0,
        overpanel: null,
        image: null,
    };

    pictureWall.instaince = null;

    $.pictureWall = function (options) {
        if (pictureWall.instaince == null) {
            pictureWall.instaince = new pictureWall(options);
        } else {
            pictureWall.instaince.options = $.extend({}, pictureWall.defaults, options);
        }
        return pictureWall.instaince;
    }
});

fruit.plugin = {};
fruit.plugin.showPopupSelect = function (opts) {
    var dialog = $('<div></div>');

    function findRowIndex(grid, data) {
        var valueFields = dialog.table.data('value-fields').split(',');
        var rows = grid.datagrid('getRows');
        return rows.indexOf(function (row) {
            for (var i = 0; i < valueFields.length; i++) {
                if (data[valueFields[i]] != row[valueFields[i]]) return false;
            }
            return true;
        });
    }

    function updateInfo() {
        var count = dialog.selectedTable.datagrid('getRows').length;
        dialog.info.html('双击选择或删除，已选中 <font color="red">' + count + '</font> 条数据');
    }

    function addSelectData(data) {
        var idx = findRowIndex(dialog.selectedTable, data);
        if (idx == -1) {
            dialog.selectedTable.datagrid('appendRow', data);
            updateInfo();
        }
    }

    function removeSelectData(data) {
        var idx = findRowIndex(dialog.selectedTable, data);
        if (idx > -1) {
            dialog.selectedTable.datagrid('deleteRow', idx);
            updateInfo();
        }
    }

    dialog.on('dblclick', '.datagrid-row', function (e) {
        var grid = $(this).closest('.datagrid-view').find('>table');
        var rowIndex = $(this).closest('.datagrid-row').attr('datagrid-row-index');
        var data = grid.datagrid('getRows')[rowIndex];
        if (data) {
            if (grid.is('.selectedTable')) {
                removeSelectData(data);
            } else {
                addSelectData(data);
            }
        }
        e.preventDefault();
        e.stopPropagation();
        return false;
    });
    dialog.dialog({
        title: opts.title,
        width: opts.width,
        height: opts.height,
        closed: false,
        cache: false,
        href: '/Plugins/Popup/' + opts.rule,
        buttons: [{
            text: '确认',
            iconCls: 'icon-ok',
            handler: function () {
                //var grid = dialog.find('[data-bind="datagrid"]');
                //var row = grid.datagrid('getSelected');
                //if (row) {
                //    var valFields = grid.data('value-fields').split(',');
                //    var retFields = grid.data('return-fields').split(',');
                //    opts.onSelect(row, valFields, retFields);
                //    dialog.dialog('close');
                //}
                var grid = dialog.find('[data-bind="datagrid"]');
                var rows = dialog.selectedTable.datagrid('getRows');
                var valFields = grid.data('value-fields').split(',');
                var retFields = grid.data('return-fields').split(',');
                $.each(rows, function () {
                    opts.onSelect(this, valFields, retFields);
                });
                dialog.dialog('close');
            }
        }],
        modal: true,
        onLoad: function () {
            dialog.table = dialog.dialog('dialog').find('table[data-bind]').datagrid(
                {
                    singleSelect:true
                });
            dialog.table.attr('readonly', 'readonly');
            dialog.info = $('<div style="float:left;position: relative;z-index:100;padding:6px;color:#666">双击选择或删除</div>').appendTo(dialog.dialog('dialog').find('.dialog-button'));
            dialog.selectedTable = $('<table class="selectedTable" style="height:100px;">' + dialog.dialog('dialog').find('[data-bind="datagrid"]').html() + '</table>').appendTo(dialog.dialog('dialog').find('.window-body')).datagrid({ border: false, rownumbers: true });
            dialog.selectedTable.attr('readonly', 'readonly');
        },
        onClose: function () {
            dialog.dialog('destroy');
        }
    });
}

// popup plugin    
$(function () {

    function createBox(target) {
        var state = $.data(target, 'popup');
        var opts = state.options;

        $(target).css('display', 'none');

        function rowIsSameKey(row1, row2) {
            if (state.popupGrid) {
                var valFields = state.popupGrid.data('value-fields').split(',');
                return row1[valFields[0]] == row2[valFields[0]];
            }
            return false;
        }

        function updateSelInfo() {
            if (state.selinfo && state.popupGrid && state.selectedRows) {
                if (state.selectedRows.length == 0) {
                    state.selinfo.text('未选择');
                    return;
                }
                var selectTexts = [];
                var textCount = 0;
                var more = false;
                var listFields = state.popupGrid.data('list-fields').split(',');
                var textField = listFields.length > 1 ? listFields[1] : listFields[0];
                for (var row of state.selectedRows) {
                    var text = row[textField];
                    if (textCount + text.length > 16) {
                        more = true;
                        break;
                    } 
                    textCount += text.length;
                    selectTexts.push(text);
                }
                state.selinfo.text('已选择 ' + selectTexts.join() + ' ' + (more ? '等，共 ' : '共 ') + state.selectedRows.length + ' 项');
            }
        }

        function popupGridOnUnselect(index, row) {

            if (!state._lockSelectedRows && state.selinfo && state.popupGrid) {
                if (state.popupGrid.datagrid('options').singleSelect) {
                    state.selectedRows = [];
                } else {
                    for (var i in state.selectedRows) {
                        var srow = state.selectedRows[i];
                        if (rowIsSameKey(row, srow)) {
                            state.selectedRows.splice(i, 1);
                            break;
                        }
                    }
                }
                updateSelInfo();
            }

            if (state.oldPopupGridOnUnselect) {
                state.oldPopupGridOnUnselect(index, row);
            }
        }

        function popupGridOnSelect(index, row) {

            if (!state._lockSelectedRows && state.selinfo && state.popupGrid) {
                if (state.popupGrid.datagrid('options').singleSelect) {
                    state.selectedRows = [row];
                } else {
                    var found = false;
                    for (var srow of state.selectedRows) {
                        if (rowIsSameKey(row, srow)) {
                            found = true;
                            break;
                        }
                    }
                    if (!found) {
                        state.selectedRows.push(row);
                    }
                }
                updateSelInfo();
            }

            if (state.oldPopupGridOnSelect) {
                state.oldPopupGridOnSelect(index, row);
            }
        }

        function popupGridOnLoadSuccess(data) {
            if (state.oldPopupGridOnLoadSuccess) {
                state.oldPopupGridOnLoadSuccess(data);
            }
            if (data && data.rows && state.popupGrid && state.selectedRows && state.selectedRows.length) {
                state._lockSelectedRows = true;
                for (var rowIndex in data.rows) {
                    var row = data.rows[rowIndex];
                    var found = false;
                    for (var srow of state.selectedRows) {
                        if (rowIsSameKey(row, srow)) {
                            found = true;
                            break;
                        }
                    }
                    if (found) {
                        state.popupGrid.datagrid('selectRow', rowIndex)
                    }
                }
                state._lockSelectedRows = false;
            }
        }

        function showPopup() {
            state.selectedRows = [];
            var dialog = $('<div></div>');
            dialog.dialog({
                title: opts.title,
                width: opts.dialogWidth,
                height: opts.dialogHeight,
                closed: false,
                cache: false,
                href: '/Plugins/Popup/' + opts.rule,
                buttons: [{
                    text: '选择',
                    iconCls: 'icon-ok',
                    handler: function () {
                        var grid = dialog.find('[data-bind="datagrid"]');
                        if (opts.onSelect === fruit.popupSelecteds || opts.onSelect === fruit.popupColumnSelecteds) {
                            if (opts.maxChoiceItems > 0 && state.selectedRows.length > opts.maxChoiceItems) {
                                alert('最多选择 ' + opts.maxChoiceItems + ' 项！');
                                return;
                            }
                            var valFields = grid.data('value-fields').split(',');
                            var listFields = grid.data('list-fields').split(',');
                            var textField = listFields.length > 1 ? listFields[1] : listFields[0];
                            var values = [], texts = [];
                            for (var row of state.selectedRows) {
                                values.push(row[valFields]);
                                texts.push(row[textField]);
                            }
                            opts.onSelect($(target), values, texts);
                            dialog.dialog('close');
                        } else {
                            var row = grid.datagrid('getSelected');
                            if (row) {
                                var valFields = grid.data('value-fields').split(',');
                                var listFields = grid.data('list-fields').split(',');
                                var retFields = grid.data('return-fields').split(',');

                                $(target).val(row[valFields[0]]);
                                var selectedText = listFields.length > 1 ? row[listFields[1]] : row[listFields[0]];
                                $.fn.popup.methods.setText($(target), selectedText);
                            
                                opts.onSelect(row, valFields, retFields, opts.textField, selectedText);

                                dialog.dialog('close');
                            }
                        }
                    }
                }],
                modal: true,
                onClose: function () {
                    dialog.dialog('destroy');
                },
                onLoad: function () {
                    var selinfo = $('<div>未选择</div>');
                    selinfo.css({ position: 'absolute', left: 5, top: 5, bottom: 5, right: 75, textAlign:'left', lineHeight:'24px' });
                    selinfo.appendTo(dialog.parent().find('.dialog-button'));
                    state.selinfo = selinfo;
                    state.popupGrid = dialog.find('[data-bind="datagrid"]');

                    var popupGridOptions = state.popupGrid.datagrid('options');

                    state.oldPopupGridOnSelect = popupGridOptions.onSelect;
                    popupGridOptions.onSelect = popupGridOnSelect;
                    state.oldPopupGridOnUnselect = popupGridOptions.onUnselect;
                    popupGridOptions.onUnselect = popupGridOnUnselect;
                    state.oldPopupGridOnLoadSuccess = popupGridOptions.onLoadSuccess;
                    popupGridOptions.onLoadSuccess = popupGridOnLoadSuccess;

                    popupGridOptions.singleSelect = !opts.multipleChoice;
                    
                    state.popupBind = fruit.databind.get(state.popupGrid);
                }
            });
            $(dialog.data('panel').panel).on('dblclick', '.datagrid-row', function () {
                dialog.dialog('options').buttons[0].handler();
            });
        }

        function selectPopupRow(state, row) {
            state.popup.removeClass('open');

            var df = state.options.displayFields.split('|');

            var text = row[df[3]];
            var value = row[df[1].split(',')[0]];

            state.textbox.val(text);
            state.$target.val(value);

            if (typeof state.options.onSelect == 'function') {

                function splitArray(a) {
                    var r = a.split(',');
                    if (r.length == 1 && r[0].length == 0) {
                        return [];
                    }
                    return r;
                }

                state.options.onSelect(row, splitArray(df[1]), splitArray(df[2]), df[3], row[df[3]]);
            }
        }

        function selectDefaultPopup(state) {
            var row = state.popup.find('.dropdown-item.selected').data('item');
            if (row && state.popup.is('.open')) {
                selectPopupRow(state, row);
            }
            else if (state.textbox.val() == '')
            {
                state.$target.val('');
            }
        }

        function updatePopup(state, data) {
            if (!state.popup) {
                state.popup = $('<div class=\"popup-dropdown\"></div>').appendTo(document.body);
            }
            var offset = state.box.offset();
            state.popup.css({left:offset.left, top:offset.top + state.box.outerHeight()});
            var table = $("<table width=\"100%\">");
            state.popup.empty().append(table);
            var first = true;
            $.each(data.rows, function () {
                var tr = $('<tr class=\"dropdown-item\">').data('item', this).appendTo(table).on('click', function () {
                    selectPopupRow(state, $(this).data('item'));
                });
                if (first) {
                    tr.addClass('selected');
                    first = false;
                }
                if (state.options.displayFields && state.options.displayFields.length) {
                    var displayFields = state.options.displayFields.split('|')[0].split(',');
                    for (var i in displayFields) {
                        var v = this[displayFields[i]];
                        if (typeof (v) !== 'function') {
                            $('<td>').text(v).appendTo(tr);
                        }
                    }
                } else {
                    for (var i in this) {
                        if (typeof this[i] != 'function') {
                            $('<td>').text(this[i]).appendTo(tr);
                        }
                    }
                }
            });
            if (state.textbox.is(':focus') && data.rows.length) {
                state.popup.addClass('open');
            } else {
                state.popup.removeClass('open');
            }
        }

        function ajaxLoad(state) {
            if (state.busy) return;
            state.busy = true;
            state.ajax = $.ajax({ url: '/Plugins/GetLookupData/' + opts.rule, data: { page: 1, rows: 10, _input: state.textbox.val() } })
                .done(function (data) {
                    updatePopup(state, data);
                })
                .always(function () {
                    state.busy = false;
                });
        }

        if (!state.box) {
            state.$target = $(target);
            state.box = $('<span class="textbox popup" style="height:20px;"><span class="textbox-addon textbox-addon-right" style="right: 0px;"><a href="javascript:void(0)" class="textbox-icon combo-arrow popup-arrow" icon-index="0" tabindex="-1" style="width: 18px; height: 20px;"></a></span><input type="text" class="textbox-text" autocomplete="off" placeholder="" style="margin-left: 0px; margin-right: 18px; padding-top: 3px; padding-bottom: 3px; min-width: 4em;"></span>').insertAfter(target);
            state.link = state.box.find('a');
            state.textbox = state.box.find('.textbox-text');
            state.textbox.validatebox(opts);
            state.busy = false;
            state.textbox.on('input', function () {
                if (state.timer) { clearTimeout(state.timer); }
                state.timer = setTimeout(function () {
                    ajaxLoad(state);
                }, 300);
            });
            state.textbox.on('keydown', function (e) {
                // up:38 down:40 esc:27 enter:13 tab:9
                if (e.keyCode == 40) {
                    if (!state.popup || !state.popup.is('.open')) {
                        ajaxLoad(state);
                    } else {
                        var cur = state.popup.find('.dropdown-item.selected');
                        if (cur.next().length) {
                            cur.removeClass('selected').next().addClass('selected');
                        }
                    }
                }
                else if (e.keyCode == 38) {
                    var cur = state.popup.find('.dropdown-item.selected');
                    if (cur.prev().length) {
                        cur.removeClass('selected').prev().addClass('selected');
                    }
                }
                else if (e.keyCode == 27) {
                    state.popup.removeClass('open');
                }
                else if (e.keyCode == 13) {
                    selectDefaultPopup(state);
                }
                else if (e.keyCode == 9) {
                    selectDefaultPopup(state);
                    state.textbox.focus();
                }
            });
            state.textbox.on('blue', function () {
                setTimeout(function () {
                    if (!state.textbox.is(':focus') && state.popup) {
                        state.popup.removeClass('open');
                    }
                }, 100);
            });
            state.link.click(function () {
                showPopup();
            });
        }
    }

    $.fn.popup = function (options, param) {
        if (typeof options == 'string') {
            var method = $.fn.popup.methods[options];
            if (method) {
                return method(this, param);
            } else {
                return this.validatebox(options, param);
            }
        }

        options = options || {};
        return this.each(function () {
            var state = $.data(this, 'popup');
            if (state) {
                $.extend(state.options, options);
            } else {
                $.data(this, 'popup', {
                    options: $.extend({}, $.fn.popup.defaults, $.fn.popup.parseOptions(this), options)
                });
            }
            createBox(this);
        });
    };

    $.fn.popup.methods = {
        options: function (jq) {
            return $.data(jq[0], 'popup').options;
        },
        dialog: function (jq) {	// get the calendar object
            return $.data(jq[0], 'popup').dialog;
        },
        setValue: function (jq, value) {
            //return jq.each(function () {
            //    setValue(this, value);
            //});
            jq.val(value);
        },
        setText: function(jq, text) {
            jq.next().find('.textbox-text').val(text);
            jq.next().find('.textbox-text').validatebox('validate');
        },
        getText: function (jq) {
            return jq.next().find('.textbox-text').val();
        },
        destroy: function (jq) {
            //$(jq).validatebox('destroy');
            $.removeData(jq, 'popup');
        }
    };

    $.fn.popup.parseOptions = function (target) {
        var t = $(target);
        return $.extend({}, $.fn.validatebox.parseOptions(target), {
        });
    };

    $.fn.popup.defaults = $.extend({}, $.fn.validatebox.defaults, {
        dialogWidth: 550,
        dialogHeight: 400,
        onSelect: $.noop,
        textField: null,
        title: '请选择',
        rule: '',
        multipleChoice: true,
        maxChoiceItems: 0
    });

    $('.easyui-popup').popup();
});

// radiobox plugin
$(function () {

    var autoId = 1;

    function updateValue(jq, value) {
        var state = createState(jq);
        state.options.value = value;
    }

    function createBox(element) {
        var jq = $(element);
        var state = createState(jq);

        if (state.options.name == null) {
            state.options.name = jq.attr('name') || ('radiobox_' + autoId++);
        }

        if ($.isArray(state.options.data)) {
            jq.empty();
            $.each(state.options.data, function () {
                var label = $('<label></label>').appendTo(jq);
                var radio = $('<input type="radio" />').attr('value', this[state.options.valueField]).attr('name', state.options.name).appendTo(label).on('click', function () {
                    updateValue(jq, this.value);
                });
                if (this[state.options.valueField] == state.options.value) {
                    radio[0].checked = true;
                }
                $('<span class="radio-label"></span>').text(this[state.options.textField]).appendTo(label);
            });
        }
    }

    function createState(jq) {
        jq = $(jq);
        var state = jq.data('radiobox');// $.data(jq, 'radiobox');
        if (!state) {
            //$.data(jq, 'radiobox', {
            jq.data('radiobox', {
                options: $.extend({}, $.fn.radiobox.defaults, $.parser.parseOptions(jq[0]))
            });
            state = jq.data('radiobox');//$.data(jq, 'radiobox');
        }
        return state;
    }

    $.fn.radiobox = function (options, param) {
        if (typeof options == 'string') {
            var method = $.fn.radiobox.methods[options];
            if (method) {
                return method(this, param);
            }
        }

        options = options || {};
        return this.each(function () {
            var state = createState($(this));
            $.extend(state.options, options);
            createBox(this);
        });
    };

    $.fn.radiobox.methods = {
        loadData: function (jq, data) {
            var state = createState(jq);
            state.options.data = data;
            createBox(jq);
        },
        setValue: function (jq, value) {
            var state = createState(jq);
            state.options.value = value;
            createBox(jq);
        },
        getValue: function (jq, value) {
            var state = createState(jq);
            return state.options.value;
        }
    };

    $.fn.radiobox.defaults = {
        name: null,
        value: null,
        valueField: 'value',
        textField: 'text',
        data: []
    };

    $.parser.plugins.push('radiobox');

});

// file plugin
$(function () {

    var autoId = 1;
    var itemBoxId = 1;
    
    function showNumber(number)
    {
        var text = '' + number;
        text = text.split('.');

        var output = [];
        while(text[0].length > 0)
        {
            var offset = text[0].length % 3 || 3;
            output.push(text[0].substr(0, offset));
            text[0] = text[0].substr(offset);
        }
        if(text.length > 1 && text[1].length > 2)
        {
            text[1] = text[1].substr(0, 2);
        }
        text[0] = output.join('\'');
        return text.join('.');
    }

    function formatBytes(bytes)
    {
        if (bytes > 1024 * 1024 * 1000) { // G
            return showNumber(bytes / 1024 / 1024 / 1000) + ' GB';
        }
        if (bytes > 1024 * 1024) { // M
            return showNumber(bytes / 1024 / 1024) + ' MB';
        }
        if (bytes > 1024) { // K
            return showNumber(bytes / 1024) + ' KB';
        }
        return bytes + ' B';
    }

    function uploadProgress(target, e) {
        var state = createState(target);
        if (!state.progress) {
            state.progress = $('<div class="file-progress"></div>').appendTo(state.text);
        }
        state.progress.css('width', e.loaded * state.text.width() / e.total);
    }

    function uploadComplete(target, e) {
        var state = createState(target);
        var data = $.parseJSON(e.target.responseText);
        state.options.value = data.fileId;
        state.button.find('.icon').removeClass('icon-upload').addClass('icon-folder_magnify').end().attr('for', state.button.data('for')).attr('title', '选择文件...');
        if (state.options.numberLimits == 1) {
            state.progress.css('width', '100%');
            state.options.files = data.files;
        } else {
            state.options.files = state.options.files.concat(data.files);
        }
        updateBox(target);
    }

    function uploadFailed(target, e) {
        console.log('error', e);
    }

    function uploadCanceled(target, e) {
        console.log('abort', e);
    }

    function addBeginUploads(target, files)
    {
        var state = createState(target);
        state.button.find('.icon').removeClass('icon-folder_magnify').addClass('icon-upload').end().attr('for', '').attr('title', '正在上传...');
        var itemBox = $('<span class="file-item"> 正在上传 ' + files.length + ' 个文件... </span>').appendTo(state.box);

        if (typeof FormData == 'function') {
            var fd = new FormData();
            fd.append('path', state.options.path);
            fd.append('fileId', state.options.value);
            fd.append('replace', 'False');
            $.each(files, function () {
                fd.append('file', this);
            });

            var xhr = new XMLHttpRequest();
            xhr.upload.addEventListener("progress", function (e) {
                var progress = itemBox.data('progress');
                if (!progress) {
                    progress = $('<span class="file-progress"></span>').appendTo(itemBox);
                    itemBox.data('progress', progress);
                }
                progress.css('width', e.loaded * itemBox.width() / e.total);
            }, false);
            xhr.addEventListener("load", function (e) {
                uploadComplete(target, e);
            }, false);
            xhr.addEventListener("error", function (e) {
                uploadFailed(target, e);
            }, false);
            xhr.addEventListener("abort", function (e) {
                uploadCanceled(target, e);
            }, false);
            xhr.open('POST', '/File/Upload');
            xhr.send(fd);
        } else {
            alert('当前浏览器不支持异步上传！');
        }

    }

    function beginUpload(target, file)
    {
        var state = createState(target);
        state.button.find('.icon').removeClass('icon-folder_magnify').addClass('icon-upload').end().attr('for', '').attr('title', '正在上传...');
        if (typeof FormData == 'function') {
            var fd = new FormData();
            fd.append('path', state.options.path);
            fd.append('fileId', state.options.value);
            fd.append('replace', 'True');
            fd.append('file', file);
            var xhr = new XMLHttpRequest();
            xhr.upload.addEventListener("progress", function (e) {
                uploadProgress(target, e);
            }, false);
            xhr.addEventListener("load", function (e) {
                uploadComplete(target, e);
            }, false);
            xhr.addEventListener("error", function (e) {
                uploadFailed(target, e);
            }, false);
            xhr.addEventListener("abort", function (e) {
                uploadCanceled(target, e);
            }, false);
            xhr.open('POST', '/File/Upload');
            xhr.send(fd);
        } else {
            alert('当前浏览器不支持异步上传！');
        }
    }

    function deleteFile(target, serial) {
        var state = createState(target);
        if (state.options.numberLimits == 1) {
            state.delButton.attr('disabled', 'disabled');
        } else {
            
        }
        var fData = { fileId: state.options.value, serial: serial };
        fruit.ajax({
            url: '/File/Delete',
            data: JSON.stringify(fData),
            success: function (data) {
                var idx = -1;
                $.each(state.options.files, function (index) {
                    if (this.Serial == serial) {
                        idx = index;
                        return false;
                    }
                });
                state.options.files.splice(idx, 1);
                updateBox(target);
            }
        });
    }

    function getFileName(name)
    {
        pos = name.lastIndexOf('\\');
        if (pos > -1) {
            name = name.substr(pos + 1);
        }
        pos = name.lastIndexOf('/');
        if (pos > -1) {
            name = name.substr(pos + 1);
        }
        return name;
    }

    function getFileExt(name) {
        var ext = '';
        var pos = name.lastIndexOf('.');
        if (pos > -1) {
            ext = name.substr(pos + 1);
        }
        return ext;
    }

    function filesSelected(target)
    {
        var state = createState(target);
        var files = state.fileInput[0].files;
        if (files) {
            if (state.options.numberLimits > 1 && state.options.files.length + files.length > state.options.numberLimits) {
                fruit.message('warning', '最多只能上传 ' + state.options.numberLimits + ' 个文件！');
                return;
            }
            var allOk = true;
            $.each(files, function () {
                if (!limitCheck(state, this)) {
                    allOk = false;
                    return false;
                }
            });
            if (allOk) {
                addBeginUploads(target, files);
            }
        }
    }

    function limitCheck(state, file)
    {

        if (state.options.typeLimits.length) {
            var ext = getFileExt(file.name);
            var found = false;
            $.each(state.options.typeLimits.split(','), function () {
                if (this.toUpperCase() == ext.toUpperCase()) {
                    found = true;
                    return false;
                }
            });
            if (!found) {
                fruit.message('warning', '只能选择 ' + state.options.typeLimits + ' 为扩展名的文件！');
                return false;
            }
        }

        if (state.options.sizeLimits) {
            if (state.options.sizeLimits < file.size) {
                fruit.message('warning', '文件大小不能大于 ' + formatBytes(state.options.sizeLimits));
                return false;
            }
        }

        return true;
    }

    function fileSelected(target)
    {
        var state = createState(target);
        var file = state.fileInput[0].files[0];
        if (file) {
            var name = getFileName(file.name);

            if (limitCheck(state, file)) {
                state.text.text(name);
                beginUpload(target, file);
            } else {
                state.fileInput.val('');
            }
        }
    }

    function updateBoxByValue(target) {
        var state = createState(target);
        if (state.options.value) {
            fruit.ajax({
                url: '/File/Get/' + state.options.value,
                success: function (data) {
                    state.options.files = data;
                    updateBox(target);
                }
            })
        } else {
            updateBox(target);
        }
    }

    function getFileExtIconClass(fn) {
        var pos = fn.lastIndexOf('.');
        if (pos > -1) {
            var ext = fn.substr(pos + 1);
            switch (ext.toLowerCase()) {
                case 'chm':
                case 'doc':
                case 'exe':
                case 'jpg':
                case 'pdf':
                case 'ppt':
                case 'rar':
                case 'txt':
                case 'xls':
                    return 'icon-ext-' + ext.toLowerCase();
                case 'docx':
                    return 'icon-ext-doc';
                case 'xlsx':
                    return 'icon-ext-xls';
                case '7z':
                case 'zip':
                    return 'icon-ext-rar';
            }
        }
        return 'icon-ext-empty';
    }

    function updateBox(target) {
        var state = createState(target);
        if (state.options.numberLimits == 1) {
            if (state.options.files.length) {
                var f = state.options.files[0];
                state.text.text(f.FileName).attr('title', '点击下载').data('href', f.Path).css('cursor', 'pointer').on('click', function () {
                    window.open($(this).data('href'));
                });
                state.delButton.data('Serial', f.Serial).show().on('click', function () {
                    deleteFile(target, $(this).data('Serial'));
                });
                state.icon.prop('class', 'file-icon ' + getFileExtIconClass(f.FileName));
            } else {
                state.text.html('&nbsp;').removeAttr('title').css('cursor', 'default').removeData('href').off('click');
                state.delButton.hide();
                state.icon.prop('class', 'file-icon');
            }
        } else {
            state.box.empty();
            if (state.options.files.length) {
                $.each(state.options.files, function () {
                    var file = this;
                    var itemBox = $('<span class="file-item"></span>').appendTo(state.box).hover(function(){$(this).addClass('file-item-hover')}, function(){$(this).removeClass('file-item-hover')});
                    var icon = $('<span class="file-icon">&nbsp;</span>').addClass(getFileExtIconClass(this.FileName)).appendTo(itemBox);
                    $('<span class="file-text"></span>').data('path', '/File/Show?fileId='+state.options.value+'&serial='+file.Serial).click(function(){window.open($(this).data('path'));}).appendTo(itemBox).text(file.FileName);
                    $('<button class="file-affix" title="删除"><span class="icon icon-bullet_cross">&nbsp;</span></button>').data('Serial', file.Serial).appendTo(itemBox).click(function () {
                        deleteFile(target, $(this).data('Serial'));
                    });
                });
            }
        }
    }

    function createBox(target) {
        var state = createState(target);
        if ($(target).is(':visible')) {
            $(target).hide();
            var id = "file_" + (autoId++);
            state.button = $('<label for="' + id + '" data-for="' + id + '" class="file-selector" title="选择文件..."><span class="icon icon-folder_magnify">&nbsp;</span></label>').insertAfter(target);
            if (state.options.numberLimits == 1) {
                state.delButton = $('<button class="file-affix" title="删除" style="display:none;"><span class="icon icon-bullet_cross">&nbsp;</span></button>').insertAfter(target);
                state.fileInput = $('<input type="file" id="' + id + '" style="position: absolute;left: -5000px;" ' + (state.options.numberLimits != 1 ? 'multiple' : '') + ' />').insertAfter(target).change(function () {
                    fileSelected(target);
                });
                state.text = $('<span class="file-text">&nbsp;</span>').insertAfter(target);
                state.icon = $('<span class="file-icon">&nbsp;</span>').insertAfter(target);
            } else {
                state.fileInput = $('<input type="file" id="' + id + '" style="position: absolute;left: -5000px;" multiple />').insertAfter(target).change(function () {
                    filesSelected(target);
                });
                state.box = $('<span class="file-box"></span>').insertAfter(target);
            }
        }
    }

    function createState(target) {
        var state = $.data(target, 'file');
        if (!state) {
            $.data(target, 'file', {
                options: $.extend({}, $.fn.file.defaults, $.parser.parseOptions(target))
            });
            state = $.data(target, 'file');
        }
        return state;
    }

    $.fn.file = function (options, param) {
        if (typeof options == 'string') {
            var method = $.fn.file.methods[options];
            if (method) {
                return method(this, param);
            }
        }

        options = options || {};
        return this.each(function () {
            var state = createState(this);
            $.extend(state.options, options);
            createBox(this);
        });
    };

    $.fn.file.cellShowCaches = {};

    var fileBackAutoId = 1;

    $.fn.file.showPictureWall = function (id, idx) {
        var cache = $.fn.file.cellShowCaches['file' + id];
        var imgIdx = 0;
        var images = [];
        for (var i = 0; i < cache.files.length; i++) {
            if (/(jpg|jpeg|gif|png)$/i.test(cache.files[i].Path)) {
                if (i == idx) {
                    imgIdx = images.length;
                }
                images.push(cache.files[i].Path);
            }
        }

        $.pictureWall({ images: images, index: imgIdx }).show();
    };

    function showFileCell(cache) {
        var result = [];
        $.each(cache.files, function (idx) {
            if (/(jpg|jpeg|gif|png)$/i.test(this.FileName)) {
                //result.push('<a href="javascript:$.fn.file.showPictureWall(\'' + cache.fileId + '\', ' + idx + ')" title="' + this.FileName + '"><span class="icon ' + getFileExtIconClass(this.FileName) + '">&nbsp;</span></a>&nbsp;');
                result.push('<a href="javascript:$.fn.file.showPictureWall(\'' + cache.fileId + '\', ' + idx + ')" title="' + this.FileName + '"><img class="image-icon" src="' + this.Path + '"></a>&nbsp;');
            } else {
                result.push('<a href="/File/Show/?fileId=' + cache.fileId + '&serial=' + this.Serial + '" target="_blank" title="' + this.FileName + '"><span class="icon ' + getFileExtIconClass(this.FileName) + '">&nbsp;</span></a>&nbsp;');
            }
        });
        result = result.join('')
        if (cache.id) {
            $.each(cache.id, function () {
                $('#' + cache.id).html(result);
            });
        }
        return result;
    }

    $.fn.file.methods = {
        getValue: function (jq) {
            var state = createState(jq[0]);
            return state.options.value;
        },
        setValue: function (jq, value) {
            var state = createState(jq[0]);
            state.options.value = value;
            updateBoxByValue(jq[0]);
        },
        createFileCell: function (value) {
            if (value) {
                var result = '';
                var cache = $.fn.file.cellShowCaches['file' + value];
                if (!cache) {
                    // 第一次,新建 cache
                    var cache = $.fn.file.cellShowCaches['file' + value] = { fileId:value, id: [] };
                    // 生成请求方案
                    $.ajax('/File/Get/' + value, {
                        method: 'POST',
                        success: function (data) {
                            cache.files = data;
                            showFileCell(cache);
                        }
                    })
                }
                if (!cache.files) {
                    // 加入 id 处理
                    var id = 'file_cell_' + fileBackAutoId++;
                    cache.id.push(id);
                    result = '<span id="' + id + '"></span>';
                } else {
                    // 直接显示
                    result = showFileCell({ fileId:value, files: cache.files });
                }
                return result;
            } else {
                return '';
            }
        }
    };

    $.fn.file.defaults = {
        value: null,
        files: [],
        typeLimits: 'jpg,gif,png',
        numberLimits: 1,
        sizeLimits: 2*1024*1024,
        path: '',
        viewMode: 'Default'
    };

    //$.parser.plugins.push('file');
    $('.easyui-file').file();
});

$(function () {
    function tryLoadDateRange(tryNum) {
        if ($.fn.daterange) {
            $('.easyui-daterange').daterange({});
        }
        else if (tryNum > 0) {
            setTimeout(function () {
                tryLoadDateRange(tryNum - 1);
            }, 100)
        }
    }
    tryLoadDateRange(3);
})