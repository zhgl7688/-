$.extend(Array.prototype, {
    find: function (pn, val) {
        var found = null;
        $.each(this, function () {
            if (typeof (pn) == 'function' ? pn(this) : this[pn] == val) {
                found = this;
                return false;
            }
        });
        return found;
    },
    indexOf:function (item) {
        for (var i = 0; i < this.length; i++) {
            if ($.isFunction(item) ? item(this[i]) : this[i] === item)
                return i;
        }
        return -1;
    },
    remove : function (item) {
        var pos = this.indexOf(item);
        if (pos > -1) {
            this.splice(pos, 1);
            return true;
        }
        return false;
    },
    copy: function () {
        var array = [];
        for (var i = 0; i < this.length; i++) {
            array.push($.extend({}, this[i]));
        }
        return array;
    },
    last: function (fn) {
        if (this.length > 0) {
            if (typeof (fn) == 'function') {
                for (var i = this.length - 1; i >= 0; i--) {
                    if (fn(this[i], i, this)) {
                        return this[i];
                    }
                }
            } else {
                return this[this.length - 1];
            }
        }
        return null;
    }
});
$.extend(String.prototype, {
    trim:function(){
        return this.replace(/(^\s*)|(\s*$)/g, "");
    },
    isGuid: function () {
        return /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/.test(this);
    }
});

$.extend({
    isString: function (str) {
        return typeof str == 'string';
    }
});

$.fn.extend({
    bindData: function (data)
    {
        this.find('[data-bind]').each(function () {
            var jopt = 'opt = {' + $(this).data('bind') + '};';
            eval(jopt);
            for (var name in opt) {
                if (name == 'text') {
                    $(this).text(data[opt[name]]);
                } else if (name == 'css') {
                    $(this).addClass(data[opt[name]]);
                }
            }
        });
        return this;
    },
    serializeObject:function () {
        var array = this.serializeArray();
        var obj = {};
        $.each(array, function () {
            obj[this.name] = this.value;
        });
        return obj;
    },
    marginHeight: function () {
        return this.outerHeight() + parseInt(this.css('marginTop')) + parseInt(this.css('marginBottom'));
    }
});

var fruit = {};

fruit.keyCode = {
    ENTER: 13,
    ESC: 27,
    UP: 38,
    DOWN: 40
};

// 表示数据未更改常量
fruit.STATE_UNCHANGE = 0;

// 表示数据是新增的常量
fruit.STATE_NEW = 1;

// 表示数据已更改的常量
fruit.STATE_CHANGED = 2;

// 表示数据已删除的常量
fruit.STATE_DELETED = 3;

// 通过路径方式获取指定属性的值
fruit.findValue = function (data, path) {
    var valuePath = path.split('.');
    var valuePathIndex = 0;
    while (data && typeof data[valuePath[valuePathIndex]] != 'undefined') {
        if (valuePathIndex == valuePath.length - 1) {
            return data[valuePath[valuePathIndex]];
        } else {
            data = data[valuePath[valuePathIndex]];
            valuePathIndex++;
        }
    }
};

// 通过路径方式设置属性值
fruit.setValue = function (data, path, value)
{
    var valuePath = path.split('.');
    var valuePathIndex = 0;
    while (data) {
        if (valuePathIndex >= valuePath.length - 1) {
            data[valuePath[valuePathIndex]] = value;
            return;
        }
        if (!data[valuePath[valuePathIndex]]) {
            data[valuePath[valuePathIndex]] = {};
        }
        data = data[valuePath[valuePathIndex++]];
    }
}

fruit.findDataSourceText = function (dataSource, field, value) {
    if (dataSource && $.isArray(dataSource[field])) {
        var item = dataSource[field].find('value', value);
        if (item) {
            return item.text;
        }
    }
    return value;
}

fruit.toDateValue = function (val) {
    if (val) {        
        return val.replace('T', ' ');
    }
    return val;
};

// 处理参数，整合对象，对应指定字符串名称参数
fruit.parseOptions = function () {
    var propStartIndex = -1;
    for (var i = 0; i < arguments.length; i++) {
        if (typeof arguments[i] == 'string') {
            propStartIndex = i;
            break;
        }
    }
    var options = {};
    if (propStartIndex == -1) {
        options = arguments[arguments.length - 1];
        for (var i = arguments.length - 2; i >= 0; i--) {
            $.extend(options, arguments[i]);
        }
        return options;
    }
    for (var i = propStartIndex; i < arguments.length; i++) {
        var name = arguments[i];
        for (var p = 0; p < propStartIndex; p++) {
            var obj = arguments[p];
            if (obj && obj.tagName) {
                var attr = $.attr(obj, name);
                if (typeof attr != 'undefined') {
                    options[name] = attr;
                    break;
                }
                var dataOpt = $(obj).data('options');
                eval('dataOpt = {' + dataOpt + '};');
                if (dataOpt && typeof dataOpt[name] != 'undefined') {
                    options[name] = dataOpt[name];
                    break;
                }
            }
            if (typeof obj != 'undefined' && typeof obj[name] != 'undefined') {
                options[name] = obj[name];
                break;
            }
        }
    }
    return options;
}


// 发送 ajax 请求
fruit.ajax = function (options) {
    options = $.extend({
        showLoading: true
    }, options);

    var ajaxDefaults = {
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        error: function (e) {
            var msg = e.responseText;
            var ret = msg.match(/^{\"Message\":\"(.*)\",\"ExceptionMessage\":\"(.*)\",\"ExceptionType\":.*/);
            if (ret != null) {
                msg = (ret[1] + ret[2]).replace(/\\"/g, '"').replace(/\\r\\n/g, '<br/>').replace(/dbo\./g, '');
            }
            else {
                try { msg = $(msg).text() || msg; }
                catch (ex) { }
            }

            fruit.message('error', msg);
        }
    };

    if (options.showLoading) {
        ajaxDefaults.beforeSend = $.showLoading;
        ajaxDefaults.complete = $.hideLoading;
    }

    $.ajax($.extend(ajaxDefaults, options));
}

// 显示信息
fruit.message = function (type, message, callback) {
    switch (type) {
        case "success":
            if (parent == window) return alert(message);
            parent.$.notify.create(message, {
                type: 'success',
                sticky: false,
                style: 'bar',
                appendTo: '#notification'
            });
            break;
        case "error":
            if (parent == window) return alert(message);
            parent.$.messager.alert('错误', message);
            break;
        case "warning":
            if (parent == window) return alert(message);
            parent.$.notify.create(message, {
                type: 'warning',
                sticky: false,
                style: 'bar',
                appendTo: '#notification'
            });
            break;
        case "information":
            parent.$.messager.show({
                title: '消息',
                msg: message
                //,showType: 'show'
            });
            break;
        case "confirm":
            return parent.$.messager.confirm('确认', message, callback);
    }

    if (callback) callback();
    return null;
};

fruit.equalIgnorecase = function (a, b) {
    if (a == b) return true;
    if (a == null && b == null) return true;
    if (a != null && b != null) {
        return a.toUpperCase() == b.toUpperCase();
    }
    return false;
};

// 读写 cookie
fruit.cookie = function (name, value) {
    var array = document.cookie.split(';');
    for (var i = 0; i < array.length; i++) {
        var kvstr = array[i];
        var kvarray = kvstr.split('=');
        if (kvarray[0].trim() == name) {
            if (arguments.length == 1) {
                return kvarray[1];
            }
            break;
        }
    }
    if (arguments.length > 1) {
        document.cookie = name + '=' + value;
    }
};

// 选项列表辅助功能集
fruit.select = {};


// 填充数据
fruit.select.fillData = function (select, rows) {
    $(select).empty();
    $.each(rows, function () {
        var $option = $('<option></option>').text(this.Text).attr('value', this.Value).appendTo(select);
        if (this.Selected) {
            $option.attr('selected', 'selected');
        }
    });
};

// 树结构相关辅助功能集
fruit.tree = {};

// 对树节点中的每一个节点调用一次回调（除非回调函数返回 false)
fruit.tree.each = function (data, callback) {
    for (var i = 0; i < data.length; i++) {
        if (callback.call(data[i], i) === false) {
            return false;
        }
        if (data[i].children) {
            if (fruit.tree.each(data[i].children, callback) === false) {
                return false;
            }
        }
    }
}

// 从树结构中删除特定节点
fruit.tree.remove = function (tree, fn) {
    for (var i = 0; i < tree.length; i++) {
        if (fn(tree[i])) {
            tree.splice(i, 1);
            i--;
        } else if (tree[i].children) {
            fruit.tree.remove(tree[i].children, fn);
        }
    }
}

// 在树中查询指定 id 的节点数据
fruit.tree.findNodeData = function (data, id, idField) {
    if (arguments.length < 3) {
        idField = '_id';
    }
    for (var i = 0; i < data.length; i++) {
        if (data[i][idField] == id || data[i]._id == id) {
            return data[i];
        }
        if (data[i].children) {
            var node = fruit.tree.findNodeData(data[i].children, id, idField);
            if (node != null) {
                return node;
            }
        }
    }
    return null;
}

// 创建用于 Tree 结构的平面数据过滤器函数，它处理自关联树数据源
fruit.tree.createLoadFilter = function (idField, parentIdField, rootParentIdValue, insertFristNode) {
    var rootValue = [];
    var rowHandle = $.noop;
    for (var i = 2; i < arguments.length; i++) {
        if (typeof arguments[i] == 'function') {
            rowHandle = arguments[i];
            if (insertFristNode === rowHandle) {
                insertFristNode = undefined;
            }
        }
        else if (arguments[i] && arguments[i].text)
        {
            insertFristNode = arguments[i];
        } else {
            rootValue.push(arguments[i]);
        }
    }

    function buildChild(data, parentNode) {
        for (var i = 0; i < data.length; i++) {
            if (data[i][parentIdField] == parentNode[idField]) {
                if (parentNode.children) {
                    parentNode.children.push(data[i]);
                } else {
                    parentNode.children = [data[i]];
                }
                rowHandle(data[i], parentNode);

                buildChild(data, data[i]);
            }
        }
    }

    return function (rows) {
        if (rows.total && rows.rows) {
            rows = rows.rows;
        }
        var newRows = [];
        for (var i = 0; i < rows.length; i++) {
            var found = false;
            for (var n = 0; n < rootValue.length; n++) {
                if (rows[i][parentIdField] == rootValue[n]) {
                    found = true;
                    break;
                }
            }
            if (found) {
                newRows.push(rows[i]);
                rowHandle(rows[i]);
                buildChild(rows, rows[i]);
            }
        }
        if (insertFristNode) {
            newRows = [insertFristNode].concat(newRows);
        }
        return newRows;
    };
}

fruit.tree.createOrganizeFilter = function () {

    function buildChild(data, parentNode) {
        for (var i = 0; i < data.length; i++) {
            if (data[i]['parentId'] == parentNode['id'] && data[i] != parentNode) {
                if (parentNode.children) {
                    parentNode.children.push(data[i]);
                } else {
                    parentNode.children = [data[i]];
                }
                if (data[i].nodeType == 'Organize') {
                    buildChild(data, data[i]);
                } else {
                    data[i].iconCls = 'icon-user';
                }
            }
        }
    }

    return function (rows) {
        var rootNode = null;
        $.each(rows, function () {
            if (this.parentId == '#root#') {
                rootNode = this;
                return false;
            }
        })

        var newRows = [rootNode];
        buildChild(rows, rootNode);
        return newRows;
    };
}

fruit.tree.collapseChildren = function (node, data) {
    $('.easyui-combotree').each(function () {
        var edata = $(this).data();
        if (edata && edata.combotree && edata.combotree.tree && edata.combotree.tree.find('>li>.tree-node').is(data[0].target)) {
            var root = $(this).combotree('tree').tree('getRoot');
            var tree = $(this).combotree('tree');
            tree.tree('collapseAll', root.target);
            return false;
        }
    });
}

fruit.ExpandChildrenTable = function (el, apiUri, tplId, ownerTableName, rowIndex, row, keysMap) {
    var ddv = $(el).datagrid('getRowDetail', rowIndex);
    if (ddv.find('table').length) return;
    // 展开新三层表
    var html = $('#' + tplId).html();
    var childrenTable = tplId.split('_')[1];
    html = html.replace(new RegExp("gridtb" + childrenTable, "g"), "gridtb" + childrenTable + "_" + rowIndex);
    ddv.html(html);
    var loadOpts = window['detailOnExpandRow' + ownerTableName].options;
    $.extend(loadOpts, {
        onLoadSuccess: function () {
            setTimeout(function () {
                $(el).datagrid('fixDetailRowHeight', rowIndex);
            }, 0);
        },
        onBeforeAdd: function (crow, grid) {
            $.each(keysMap, function (key) {
                crow[this] = row[key];
            });
            if (row._id_) {
                crow._pid_ = row._id_;
            }
        }
    });
    var ddb = new fruit.databind(ddv[0], loadOpts);
    $(el).datagrid('fixDetailRowHeight', rowIndex);
    var childrenGrid = ddb.binds[0];
    childrenGrid.jq.datagrid({
        height:150,
        method: 'GET',
        url: apiUri + '?' + $.map(keysMap, function (val, key) { return val + '=' + row[key] }).join('&'),
        onResize: function () {
            $(el).datagrid('fixDetailRowHeight', rowIndex);
        }
    });
}

fruit.resizeHandle = function () {
    var width = $(window).width();
    var style = document.createElement("style");
    style.type = 'text/css';
    style.appendChild(document.createTextNode(''));
    var sheet = style.sheet;
    var selector = '.val>.z-txt,.val>.textbox,.val>textarea,.val>.edui-default';
    var rules = 'max-width:calc(' + width + 'px - 10em);';
    var index = -1;
    style.appendChild(document.createTextNode(selector + '{' + rules + '}'));
    document.head.appendChild(style);

    $('#tt').tabs('resize');
}
$(function () {
    $(window).on('resize.fruit', fruit.resizeHandle);
    $(window).triggerHandler('resize.fruit');
    // 搜索面板回车自动化处理
    $('#condition').on('keydown', function (e) {
        if (e.keyCode == 13) {
            $('[data-bind="click:\'searchClick\'"]').trigger('click');
            e.preventDefault();
        }
    })
});
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
// 提供“关联的表单”功能
$(function () {
    var Relation = function (element) {
        var r = this;
        this.element = element;
        this.jq = $(element);

        this.menu = $(this.jq.attr('menu'));
        this.menu.html('<div><img src="/Content/easyui/themes/default/images/loading.gif" align="absmiddle">正在查找关联...</div>');
        this.status = 0; // 0 - none, 1 - loading, 2 - loaded, 3 - loadfail

        r.load = function () {
            r.status = 1;
            var url = "/api/Xt/" + r.jq.attr('code') + '?context='+r.jq.attr('context');
            var data = fruit.databind.root.getData('form');
            fruit.ajax({
                method: 'POST',
                url: url,
                data: JSON.stringify(data),
                success: function (data) {
                    r.loaded(data);
                }
            });
        };

        r.loaded = function (data) {
            r.status = 2;
            console.log('loaded', data);
            var sb = [];
            if (data.length) {
                r.menu.empty();
                $.each(data, function () {
                    var menu = this;
                    var mi = { text: this.Title };
                    if (menu.IconClass) {
                        mi.iconCls = this.IconClass;
                    }
                    if (!menu.Enabled) {
                        mi.disabled = true;
                    }
                    mi.onclick = function () {
                        if (menu.Blank) {
                            window.open(menu.Url);
                        } else {
                            parent.showDocTab(menu.IconClass, menu.Title, menu.Url);
                        }
                    };
                    r.menu.menu('appendItem', mi);
                });
            } else {
                r.menu.html('<div>当前没有可用关联表单</div>');
            }
        };

        this.menu.menu({
            onShow: function () {
                console.log('onShow');
                switch (r.status) {
                    case 0: // none, start loading
                        r.load();
                        break;
                }
            }
        });
    };
    $('#a_relations').each(function () {
        new Relation(this);
    });
});
// 提供报表相关功能支持
+function () {
    "use strict";

    var options = {lastUid:'', reports:[]};

    var rp = fruit.report = {
        showDetialReports: function(index, row) {
            // 主从表专用，主表行点击事件处理
            var tabs = $('#subreports').show();
            var tabsMask = $('#subreports_mask').hide();

            var w = $(window).innerWidth();
            var h = tabs.innerHeight();

            $('#subreports div[drill-report-uid]').each(function () {
                var uid = $(this).attr('drill-report-uid');

                var url = location.href.split('#')[0] + '/' + uid;
                var params = {};
                for (var key in row) {
                    params['parent_' + key] = row[key];
                }
                $(this).empty();
                var panel = $('<div></div>').addClass('drill-report-panel').data('uid', uid).appendTo(this).panel({
                    href: url,
                    queryParams: params,
                    border: false,
                    doSize: true,
                    onLoad: function () {
                        var db = new fruit.databind(panel[0]);
                        var fn = uid + '_load';
                        if ($.isFunction(window[fn])) {
                            window[fn](db);
                        } else {
                            throw new Error('未找到子视图初始化函数 ' + fn);
                        }
                        $(this).find('table[data-bind]').datagrid('resize', { width: w - 8, height: h - 68 });
                    },
                    onResize: function () {
                        console.log(w, h);
                        $(this).find('table[data-bind]').datagrid('resize', { width: w - 8, height: h - 68 });
                    }
                });

            });

        },
        isDetialReport: function(uid) {
            // 指定报表 uid 是否为主从报表的子报表
            return !!$('#subreports div[drill-report-uid="' + uid + '"]').length;
        },
        hasDetialReports: function() {
            // 当前报表是否有主从报表
            return !!$('#subreports').length;
        },
        showDrillReport: function (uid, title, row, jq) {
            if (rp.isDetialReport(uid)) {
                $('#subreports').tabs('select', title);
                rp.showDetialReports(null,row);
                return;
            }
            if (!rp.hasDrillReportRoot()) {
                rp.createDrillReportRoot();
            }
            rp.pushItem(uid, title, row);
        },
        hasDrillReportRoot: function () {
            return !!options.root;
        },
        createDrillReportRoot: function () {
            options.root = $('<div class="drill-report"></div>').appendTo(document.body);
            options.nav = $('<ul class="drill-report-nav"><li><a href="#" data-uid="">主视图</a></li></ul>').appendTo(options.root);
            options.content = $('<div class="drill-report-content"></div>').appendTo(options.root);
        },
        removeDrillReportRoot: function () {
            options.root.empty().remove();
            options.root = null;
        },
        pushItem: function(uid, title, row)
        {
            // 更新最后 uid 信息
            options.lastUid += '/' + uid;
            // 加入报表导航栏
            var li = $('<li><a href="#" data-uid="' + options.lastUid + '">' + title + '</a></li>').appendTo(options.nav);
            // 加入内容面板
            var url = location.href.split('#')[0] + '/' + uid;
            var params = {};
            for (var key in row) {
                params['parent_' + key] = row[key];
            }
            var panel = $('<div></div>').addClass('drill-report-panel').data('uid', options.lastUid).appendTo(options.content).panel({
                href: url,
                queryParams: params,
                border: false,
                doSize: true,
                onLoad: function () {
                    var db = new fruit.databind(panel[0]);
                    var fn = uid + '_load';
                    if ($.isFunction(window[fn])) {
                        window[fn](db);
                    } else {
                        throw new Error('未找到子视图初始化函数 ' + fn);
                    }
                }
            });
            options.reports.push({
                uid: uid,
                li: li,
                panel: panel
            });
        },
        popItem:function(){
            if (options.reports.length) {
                var report = options.reports.pop();

                var path = options.lastUid.split('/');
                path.pop();
                options.lastUid = path.join('/');

                report.li.empty().remove();
                report.panel.empty().remove();

                if (!options.reports.length) {
                    rp.removeDrillReportRoot();
                }
                return;
            }
        },
        popToItem: function(uid)
        {
            while (options.reports.length > 0 && options.reports.last().uid != uid) {
                rp.popItem();
            }
        }
    };

    $(document).on('click', '.drill-report-nav a[data-uid]', function () {
        var uid = $(this).data('uid');
        if (uid == options.lastUid) {
            return;
        }
        var upath = uid.split('/');
        if (upath.length == 1) {
            rp.removeDrillReportRoot();
            return;
        }

        rp.popToItem(uid);
    });
}();
fruit.databind.default.binder.click = function (databind, target, options) {
    var b = this;
    this.db = databind;
    this.jq = $(target);
    this.options = fruit.parseOptions(options, databind.options, 'click');

    if (this.jq.data('linkbutton')) {
        this.jq.linkbutton({
            onClick: function (e) {
                b.db.invoke(b.options.click, e);
            }
        });
    } else {
        this.jq.on('click', function (e) {
            b.db.invoke(b.options.click, e);
        });
    }
};

fruit.databind.default.binder.datasource = function (databind, target, options) {
    var b = this;
    this.db = databind;
    this.jq = $(target);
    this.options = fruit.parseOptions(options, databind.options, 'datasource');

    this.bindData = function (data) {
        if (data == null || data == undefined) return;
        if (typeof b.options.datasource == 'string') {
            data = fruit.findValue(data, b.options.datasource);
            if (typeof data == 'undefined') return;
        }
        if (b.jq.is('.easyui-combobox')) {
            b.jq.combobox('loadData', data);
        }
        else if (b.jq.is('.easyui-radiobox')) {
            b.jq.radiobox('loadData', data);
        }
        else if (b.jq.is('.easyui-combotree')) {
            b.jq.combotree('loadData', data);
        }
    }

    if ($.isArray(this.options.datasource)) {
        b.bindData(this.options.datasource);
    }
};

fruit.databind.default.binder.readOnly = function (databind, target, options) {
    var b = this;
    this.db = databind;
    this.jq = $(target);
    this.options = fruit.parseOptions(options, databind.options, 'readOnly');

    this.readOnly = function(readonly) {
        if (b.jq.is('.easyui-datebox')) {
            b.jq.datebox({ disabled: readonly });
        } else {
            b.jq.prop('disabled', 'disabled');
        }
    }

    if (this.options.readOnly === true) {
        b.readOnly(b.options.readOnly);
    }
};

fruit.databind.default.binder.text = function (databind, target, options) {
    var b = this;
    this.db = databind;
    this.jq = $(target);
    this.options = fruit.parseOptions(options, databind.options, 'text');

    this.setText = function (text) {
        for (var i = 0; i < fruit.databind.default.binder.text.setter.length; i++) {
            var setter = fruit.databind.default.binder.text.setter[i];
            if (b.jq.is(setter.selector)) {
                return setter.set(b.jq, text);
            }
        }
        b.jq.text(text);
    }

    this.bindData = function (data) {

        var text = fruit.findValue(data, b.options.text);
        if (typeof (text) != 'undefined') {
            b.setText(text);
        }
    }

    this.getData = function (obj) {
        if (b.options.text && b.options.text.split('.').length == 2) {
            var text = null;
            for (var getter of fruit.databind.default.binder.text.getter) {
                if (b.jq.is(getter.selector)) {
                    fruit.setValue(obj, b.options.text, getter.get(b.jq));
                    return;
                }
            }
        }
    }
};

fruit.databind.default.binder.text.setter = [
    { selector: '.easyui-popup', set: function (jq, text) { $.fn.popup.methods.setText(jq, text); } }
];

fruit.databind.default.binder.text.getter = [
    { selector: '.easyui-popup', get: function (jq) { return $.fn.popup.methods.getText(jq); } }
]

fruit.databind.default.binder.readOnly = fruit.databind.default.binder.disable = function (databind, target, options) {
    var b = this;
    this.db = databind;
    this.jq = $(target);
    this.options = fruit.parseOptions(options, databind.options, 'readOnly', 'disable');

    
    this.bindData = function (data) {
        if ($.isString(b.options.readOnly) || $.isString(b.options.disable)) {
            var boolVal = !!fruit.findValue(data, b.options.readOnly || b.options.disable);
            b.setReadOnly(boolVal);
        }
    }

    this.setReadOnly = function (readOnly) {
        if (b.jq.data('datebox')) {
            b.jq.datebox({ 'disabled': readOnly });
        } else if (b.jq.data('linkbutton')) {
            //b.jq.linkbutton({ 'disabled': readOnly });
            b.jq.linkbutton(readOnly ? 'disable' : 'enable');
        } else if (b.jq.data('combobox')) {
            b.jq.combobox({ 'disabled': readOnly });
        } else if (b.jq.data('numberbox')) {
            b.jq.numberbox({ 'disabled': readOnly });
        } else {
            if (readOnly) {
                b.jq.attr('disable', 'disable').attr('readonly', 'readonly');
            } else {
                b.jq.removeAttr('disable').removeAttr('readonly');
            }
        }
    }

    if (b.options.readOnly === true || b.options.readOnly === false) {
        b.setReadOnly(b.options.readOnly);
    }
    else if (b.options.disable === true || b.options.disable === false) {
        b.setReadOnly(b.options.disable);
    }
}

fruit.databind.default.binder.value = function (databind, target, options) {
    var b = this;
    this.db = databind;
    this.jq = $(target);
    this.options = fruit.parseOptions(options, databind.options, 'value', 'set');

    this.bindData = function (data) {
        var val = fruit.findValue(data, b.options.value);
        if (typeof (val) != 'undefined') {
            b.setValue(val);
        }
    };

    this.getValue = function () {
        for (var i = 0; i < fruit.databind.default.binder.value.getter.length; i++) {
            var getter = fruit.databind.default.binder.value.getter[i];
            if (b.jq.is(getter.selector)) {
                return getter.get(b.jq);
            }
        }
        return b.jq.val();
    }

    this.setValue = function (val) {
        if (b.options.set === false) return;
        for (var i = 0; i < fruit.databind.default.binder.value.setter.length; i++) {
            var setter = fruit.databind.default.binder.value.setter[i];
            if (b.jq.is(setter.selector)) {
                return setter.set(b.jq, val);
            }
        }
        b.jq.val(val);
    }

    this.getData = function (obj) {
        var value = b.getValue();
        var name = b.options.value;
        var path = name.split('.');
        if ($.isPlainObject(obj) && name) {
            var ref = obj;
            for (var i = 0; i < path.length - 1; i++) {
                ref[path[i]] = ref[path[i]] || {};
                ref = ref[path[i]];
            }
            ref[path[path.length - 1]] = value;
        }
        return value;
    }

    this.clearValue = function (name) {
        if (name) {
            if (b.options.value.indexOf(name) == 0) {
                b.setValue('');
            }
        } else {
            b.setValue('');
        }
    }
};

fruit.databind.default.binder.value.getter = [
    {
        selector: '[fruit-type="SelectUser"]',
        get: function (jq) {
            var checkedNodes = jq.combotree('tree').tree('getChecked');
            var userIds = [];
            $.each(checkedNodes, function () {
                if (this.nodeType != 'Organize') {
                    userIds.push(this.id);
                }
            });
            return userIds.join();
        }
    },
    {
        selector: '[richtextbox]', get: function (jq) {
            if (!jq.data('ue')) {
                jq.data('ue', UE.getEditor(jq.attr('richtextbox')));
                return '';
            } else {
                return jq.data('ue').getContent();
            }
        }
    },
    { selector: '.easyui-numberbox', get: function (jq) { return jq.numberbox('getValue'); } },
    { selector: '.easyui-combobox', get: function (jq) { return jq.combobox('getValue'); } },
    { selector: '.easyui-combotree', get: function (jq) { return jq.combotree('getValue'); } },
    { selector: '.easyui-datebox', get: function (jq) { return jq.datebox('getValue'); } },
    { selector: '.easyui-datetimebox', get: function (jq) { return jq.datetimebox('getValue'); } },
    { selector: '.easyui-timebox', get: function (jq) { return jq.timebox('getValue'); } },
    { selector: '.easyui-timespinner', get: function (jq) { return jq.timespinner('getValue'); } },
    { selector: '[type="checkbox"]', get: function (jq) { return jq[0].checked ? 1 : 0; } },
    { selector: '.easyui-radiobox', get: function (jq) { return jq.radiobox('getValue'); } },
    { selector: '.easyui-file', get: function (jq) { return jq.file('getValue'); } },
    { selector: '.easyui-validatebox', get: function (jq) { return jq.val(); } }
];

fruit.databind.default.binder.value.setter = [
    {
        selector: '[richtextbox]', set: function (jq, val) {
            if (!jq.data('ue')) {
                jq.data('ue', UE.getEditor(jq.attr('richtextbox')));
                jq.data('ue').ready(function () {
                    if (val) {
                        jq.data('ue').setContent(val);
                    }
                });
            } else {
                if (val) {
                    jq.data('ue').setContent(val);
                }
            }
        }
    },
    { selector: '.easyui-numberbox', set: function (jq, val) { return jq.numberbox('setValue', val); } },
    { selector: '.easyui-combobox', set: function (jq, val) { return jq.combobox('setValue', val); } },
    { selector: '.easyui-combotree', set: function (jq, val) { return jq.combotree('setValue', val); } },
    { selector: '.easyui-datebox', set: function (jq, val) { return jq.datebox('setValue', val); } },
    { selector: '.easyui-datetimebox', set: function (jq, val) { val && jq.datetimebox('setValue', val.replace('T', ' ')); } },
    { selector: '.easyui-timebox', set: function (jq, value) { return jq.timebox('setValue', value); } },
    { selector: '.easyui-timespinner', set: function (jq, value) { return jq.timespinner('setValue', value); } },
    { selector: '[type="checkbox"]', set: function (jq, val) { return jq[0].checked = (typeof(val)=='boolean' ? val : !!parseInt(val)); } },
    { selector: '.easyui-radiobox', set: function (jq, val) { return jq.radiobox('setValue', val); } },
    { selector: '.easyui-file', set: function (jq, val) { return jq.file('setValue', val); } },
    { selector: '.easyui-validatebox', set: function (jq, val) { jq.val(val); jq.validatebox('validate'); } }
];
fruit.databind.default.binder.form = function (databind, target, options) {
    var b = this;
    this.db = databind;
    this.jq = $(target);
    this.options = fruit.parseOptions(options, databind.options, 'form', 'saveUrl', 'urlArgs');

    this.bindData = function (data) {
        if ($.isPlainObject(data)) {
            this.loadData(data);
        }
    }

    this.loadData = function (data) {
        b.jq.form('load', data);
    }

    this.validate = function () {
        return b.jq.form('validate');
    }

    this.save = function () {
        var url = b.options.saveUrl;
        if (b.options.urlArgs) {
            url += b.options.urlArgs;
        }
        b.jq.showLoading('正在提交数据…');
        fruit.ajax({
            method: 'post',
            url: url,
            data: JSON.stringify(b.jq.serializeObject()),
            success: function (data) {
                b.jq.hideLoading();
                b.db.invoke('onSave', data);
            }
        });
    }
};
fruit.databind.default.binder.checked = function (databind, target, options) {
    $(target).click(function () {
        databind.invoke(options.checked, $(this)[0].checked);
    });
};

fruit.databind.default.binder.checklist = function (databind, target, options) {
    var b = this;
    databind.tree = this;
    this.db = databind;
    this.jq = $(target);
    this.options = fruit.parseOptions(options, target, databind.options, 'checklist', 'url', 'saveUrl', 'urlArgs', 'idField', 'textField', 'checkField', 'checkValue', 'uncheckValue', 'method');
    this.options.itemTemplate = this.options.itemTemplate || this.jq.html();
    this.options = $.extend({ idField: 'Value', textField: 'Text', checkField: 'Selected', checkValue: true, uncheckValue: false }, this.options);
    this.jq.empty();
    b.jq.addClass('fruit-checklist');

    this.bindData = function (data) {
        if (typeof b.options.checklist == 'string') {
            data = data[b.options.checklist];
        }
        if ($.isArray(data)) {
            b.loadData(data);
        }
    }

    this.loadData = function (data) {
        b.jq.empty();
        b.options.data = data;
        $.each(data, function () {
            var item = $(b.options.itemTemplate).bindData(this).attr('item-id', this[b.options.idField]).appendTo(b.jq);
            if (this[b.options.checkField] == b.options.checkValue) {
                item.addClass('selected');
            }
        });
    }

    this.getChangeRows = function () {
        var rows = [];
        $.each(b.options.data, function () {
            if (this._row_state) {
                rows.push(this);
            }
        });
        return rows;
    };

    this.checkAll = function (checked) {
        $.each(b.options.data, function () {
            var isChecked = this[b.options.checkField] == b.options.checkValue;
            if (isChecked != checked) {
                b.check(this[b.options.idField], checked);
            }
        });
    };

    this.check = function (id, checked) {
        var node = b.options.data.find(b.options.idField, id);
        if (node) {
            node.Selected = checked;
            node._row_state = fruit.STATE_CHANGED;
            var jqNode = b.jq.find('[item-id="' + node[b.options.idField] + '"]');
            if (checked) {
                jqNode.addClass('selected');
            } else {
                jqNode.removeClass('selected');
            }
        }
    };

    this.setSelecteds = function (ids) {
        b.jq.find('li').removeClass('selected');
        $.each(ids, function () {
            b.jq.find('li[item-id="' + this + '"]').addClass('selected');
        });
    };

    this.getSelecteds = function () {
        var ids = [];
        b.jq.find('.selected[item-id]').each(function () {
            ids.push($(this).attr('item-id'));
        });
        return ids;
    }

    this.refresh = function () {
        if (b.options.url) {
            var url = b.options.url;
            if (b.options.urlArgs) {
                url += b.options.urlArgs;
            }
            b.jq.showLoading();
            var cfg = {
                url: url,
                success: function (data) {
                    b.jq.hideLoading();
                    b.loadData(data);
                }
            };
            if (b.options.method) {
                cfg.method = b.options.method;
            }
            fruit.ajax(cfg);
        }
    }

    this.save = function () {
        var saveUrl = b.options.saveUrl;
        if (b.options.urlArgs) {
            saveUrl += b.options.urlArgs;
        }
        var data = this.getChangeRows();
        fruit.ajax({
            url: saveUrl,
            data: JSON.stringify(data),
            success: function (data) {
                b.db.options.onSave(data);
            }
        });
    }

    this.jq.on('click', '[item-id]', function () {
        var checked = $(this).is('.selected');
        b.check($(this).attr('item-id'), !checked);
    });

    this.refresh();
};
fruit.databind.default.binder.tree = function (databind, target, options) {
    var b = this;
    databind.tree = this;
    b.db = databind;
    b.jq = $(target);
    b.options = fruit.parseOptions(options, target, databind.options, 'tree', 'loadFilter', 'url', 'saveUrl', 'idField', 'treeField', 'iconField', 'parentField', 'parentNameField', 'fixSize', 'onSelect');
    b.options = $.extend({ idField: 'Id', treeField: 'Name', edit_id: null, onSelect: $.noop }, this.options);

    b.refresh = function () {
        b.jq.tree('reload');
    }

    b.jq.tree({
        onSelect: function (node) {
            b.options.onSelect(node);
        }
    });
};
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
fruit.databind.default.binder.orgtree = function (databind, target, options) {
    var b = this;
    databind.tree = this;
    this.db = databind;
    this.jq = $(target);
    this.options = fruit.parseOptions(options, target, databind.options, { nodeDblClick: $.noop }, 'url', 'idField', 'parentField', 'treeField', 'nodeDblClick', 'checkbox');

    if (this.options.checkbox) {
        this.jq.addClass('org-tree-checkbox');
        this.options.selectedNodes = [];
    }

    this.loadData = function (data) {

        b.options.data = data;

        function buildTreeNode(data, level, parentElement, parentNodeId) {
            var rootTbl, rootTrs;
            var showCount = 0;
            var startTrIdx = level > 0 ? 2 : 0;
            for (var i = 0; i < data.length; i++) {
                var row = data[i];
                if (row[b.options.parentField] == parentNodeId) {
                    showCount++;

                    rootTbl = $('<table cellspacing=0 cellpadding=0 align="center"><tr></tr><tr></tr><tr></tr></table>');

                    if (level > 0) {
                        $('<tr>').appendTo(rootTbl);
                        $('<tr>').appendTo(rootTbl);
                    }

                    rootTrs = rootTbl.find('tr');

                    if (level > 0) {
                        if (showCount == 1) {
                            $('<td><table cellspacing=0 cellpadding=0 align="center" width="100%"><tr><td style="height:3px"></td><td style="height:3px;background-color:#000;"></td></tr></table></td>').appendTo(rootTrs[0]);
                        } else {
                            $('<td style="height:3px;background-color:#000;"></td>').appendTo(rootTrs[0]);
                        }
                        $('<td class="orgtree-vline"></td>').appendTo(rootTrs[1]);
                    }

                    var nodeEle = $('<table class="orgtree-node" cellspacing="0" cellpadding="0" align="center"><tr><td align="center">' + row[b.options.treeField] + '</td></tr></table>').attr('node-id', row[b.options.idField]);
                    var nodeCell = $('<td valign="top" align="center"></td>');
                    nodeCell.append(nodeEle).appendTo(rootTrs[startTrIdx]);
                    $('<td class="orgtree-vline"></td>').appendTo(rootTrs[startTrIdx + 1]);
                    var childTbl = $('<td valign="top"><table cellspacing=0 cellpadding=0 align="center"><tr></tr></table></td>').appendTo(rootTrs[startTrIdx + 2]);

                    if (level == 0) {
                        rootTbl.appendTo(parentElement);
                    } else {
                        $('<td valign="top"></td>').appendTo(parentElement).append(rootTbl);
                    }

                    var childCount = buildTreeNode(data, level + 1, childTbl.find('tr')[0], row[b.options.idField]);
                    if (level > 0) {
                        if (childCount == 0) {
                            var ec = rootTrs.length;
                            $(rootTrs[ec - 1]).remove();
                            $(rootTrs[ec - 2]).remove();
                        } else if (childCount == 1) {
                            //$(rootTrs[0]).find('td:last').css('backgroundColor', 'red');
                        }
                    }
                }
            }

            if (level > 0) {
                if (showCount == 1) {
                    $(rootTrs[1]).remove();
                    $(rootTrs[0]).remove();
                } else if (showCount > 1) {
                    $(rootTrs[0]).html('<td><table cellspacing=0 cellpadding=0 align="center" width="100%"><tr><td style="height:3px;background-color:#000;"></td><td style="height:3px;"></td></tr></table></td>');
                }
            }
            return showCount;
        }

        this.jq.empty();
        buildTreeNode(data, 0, this.jq, '0');

    }

    this.getData = function () {
        return b.options.data;
    }

    this.getTreeData = function (rowHandler) {
        if (!$.isFunction(rowHandler)) {
            rowHandler = function (row) {
                row.id = row[b.options.idField];
                row.text = row[b.options.treeField];
            }
        }
        var rows = b.options.data;
        var filter = fruit.tree.createLoadFilter(b.options.idField, b.options.parentField, '0', rowHandler)
        return filter(rows);
    }

    this.select = function (id) {
        if (this.options.checkbox) {
            var hasSelected = this.options.selectedNodes.indexOf(function (i) { return i == id });
            if (hasSelected > -1) {
                this.options.selectedNodes.splice(hasSelected, 1);
                this.jq.find('.orgtree-node[node-id="' + id + '"]').removeClass('orgtree-node-selected');
            } else {
                this.options.selectedNodes.push(id);
                this.jq.find('.orgtree-node[node-id="' + id + '"]').addClass('orgtree-node-selected');
            }
        } else {
            this.jq.find('.orgtree-node').removeClass('orgtree-node-selected');
            b.options.selectedNode = b.options.data.find(b.options.idField, id);
            this.jq.find('.orgtree-node[node-id="' + id + '"]').addClass('orgtree-node-selected');
        }
    }

    this.getSelected = function () {
        return b.options.selectedNode;
    }

    this.getSelecteds = function () {
        return b.options.selectedNodes;
    }

    this.setSelecteds = function (ids) {
        if ($.isString(ids)) {
            ids = ids.splice(',');
        }
        this.options.selectedNodes = ids;
        this.jq.find('.orgtree-node').removeClass('orgtree-node-selected');
        $.each(ids, function () {
            b.jq.find('.orgtree-node[node-id="' + this + '"]').addClass('orgtree-node-selected');
        });
    }

    this.delete = function () {
        if (b.options.selectedNode) {
            b.options.data.remove(b.options.selectedNode);
            b.options.selectedNode = null;

            b.loadData(b.options.data);
        }
    }
    
    this.refresh = function () {
        if (b.options.url) {
            b.jq.showLoading();
            fruit.ajax({
                method: 'get',
                url: b.options.url,
                success: function (data) {
                    b.jq.hideLoading();
                    b.loadData(data);
                }
            })
        }
    }

    this.jq.on('click', '.orgtree-node', function () {
        b.select.call(b, $.attr(this, 'node-id'));
    });
    this.jq.on('dblclick', '.orgtree-node', function () {
        var id = $.attr(this, 'node-id');
        if ($.isFunction(b.options.nodeDblClick)) {
            b.options.nodeDblClick.call(b, id);
        } else if ($.isString(b.options.nodeDblClick)) {
            b.db.invoke(b.options.nodeDblClick, id);
        }
        //b.select($.attr(this, 'node-id'));
    })
    
    this.refresh();
}
fruit.databind.default.binder.cascadeFilter = function (databind, target, options) {
	var db = databind;
	var $element = $(target);
	var opt = options.cascadeFilter;

	var parentValueName = 'form.' + opt.ParentField;
	var dataIsLoaded = false;

	var parentCombobox = db.jq.find('.easyui-combobox[data-bind*="value:\'' + parentValueName + '\'"]');

	function getParentValue() {
		return parentCombobox.combobox('getValue');
	}

	function updateSources() {
		var newValue = getParentValue();
		var isNull = typeof (newValue) == 'undefined' || newValue == null || newValue == '';
		var comboboxData = $element.data('combobox');
		if (!dataIsLoaded && comboboxData.data.length > 0) {
			$element.data('combobox').allData = $element.data('combobox').data;
			dataIsLoaded = true;
		}
		var allData = $element.data('combobox').allData;
		if ($.isArray(allData)) {
			var newData = [];
			if (isNull) {
				if (opt.FullIfParentNull) {
					newData = allData;
				}
			} else {
				$.each(allData, function () {
					if (this[opt.ConditionField] == newValue) {
						newData.push(this);
					}
				});
			}
			$element.combobox('loadData', newData);
			if (newData.length) {
				$element.combobox('setValue', newData[0].value);
			}
		}
	}

	$element.combobox({
		onLoadSuccess: function () {
			dataIsLoaded = true;
			$element.data('combobox').allData = $element.data('combobox').data;
		}
	});

	if (parentCombobox && parentCombobox.length) {
		parentCombobox.combobox({
			onChange: function (newValue, oldValue) {
				updateSources(newValue);
			}
		})
	}
}
$(function () {
    // 定位工具栏搜索按钮区
    var condition = $('.z-toolbar>.condition_buttons');
    if (condition.length > 0) {
        var searchSeparator = $('<div class="datagrid-btn-separator"></div>').insertBefore(condition.find('a:first-child'));
        var plSearchSchema = $('<div class="search-scheme-plane">').insertBefore(searchSeparator);
        var btnPopup = $('<a href="#"><span class="l-btn-left l-btn-icon-left"><span class="l-btn-text">搜索方案</span><span class="l-btn-icon icon-application_form_magnify">&nbsp;</span></span></a>').appendTo(plSearchSchema);
        var popup = $('<div class="sspopup"></div>').appendTo(plSearchSchema);

        var empty = $('<div class="ssempty">当前没有保存搜索方案。</div>').appendTo(popup);
        var list = $('<div class="sslist"></div>').appendTo(popup);
        var newbox = null;
        var newData = null;

        plSearchSchema.on('click', '.ssitem', function () {
            var item = $(this).data();
            if (item && item.Data) {
                var data = JSON.parse(item.Data);

                var databindHost = $(plSearchSchema).closest('[fruit="databind"]');
                var db = databindHost.data('fruit_databind');

                db.loadData({ serach: data });
                db.invoke('searchClick');
            }
        });

        var btnAdd = $('<a href="#" class="easyui-linkbutton l-btn l-btn-small l-btn-plain"><span class="l-btn-left l-btn-icon-left"><span class="l-btn-text">新建搜索方案</span><span class="l-btn-icon icon-add">&nbsp;</span></span></a>').appendTo(popup).on('click', function () {

            // 获取所在的绑定上下文
            var databindHost = $(this).closest('[fruit="databind"]');
            var db = databindHost.data('fruit_databind');

            // 获取搜索控件数据
            newData = db.getData('serach');
            
            // 测试数据是否都为空
            var isemtpy = true;
            $.each(newData, function (key) {
                var val = newData[key];
                var propType = typeof val;
                if (propType == 'string') {
                    if (val.length > 0) {
                        isemtpy = false;
                        return false;
                    }
                }
            });

            if (isemtpy) {
                fruit.message('warning', '至少设置一个条件才能保存搜索方案！');
                return;
            }

            plSearchSchema.addClass('ss-newbox');
            empty.hide();
            list.show();
            newbox = $('<div class="ssitem"></div>').appendTo(list);
            $('<input type="text" placeholder="搜索方案名称" class="ss-new" title="Enter:保存新方案&#10;Esc:取消" />').appendTo(newbox).focus().on('keydown', function (e) {
                if (e.keyCode == 27) {
                    leveNewBox();
                } else if (e.keyCode == 13) {
                    checkNewBox();
                }
            }).on('blur', function(){
                leveNewBox();
            });
            btnAdd.hide();
        });

        function checkNewBox() {
            var title = newbox.find('input').val();
            // 必填
            if (title.length == 0) {
                fruit.message('warning', '搜索方案名称是必须的！');
                return;
            }
            // 重复
            if (list.find(".sstitle:contains('" + title + "')").length > 0) {
                fruit.message('warning', '此名称已存在或包括在其它搜索方案名称中！');
                return;
            }
            newbox.find('input').attr('disable', 'disable');
            $.ajax({
                url: '/Plugins/SearchScheme',
                method: 'POST',
                data: {
                    method: 'add',
                    title: title,
                    pageCode: location.pathname.toUpperCase(),
                    data: JSON.stringify(newData)
                }, dataType: 'json'
            })
            .done(function (data) {
                leveNewBox();
                loadScheme();
            });
        }

        function leveNewBox() {
            plSearchSchema.removeClass('ss-newbox');
            btnAdd.show();
            newbox.remove();
            if (list.find('>*').length == 0) {
                list.hide();
                empty.show();
            }
        }

        function addScheme(ss) {
            var item = $('<div class="ssitem"></div>').appendTo(list).data(ss);
            $('<div>').addClass('sstitle').text(ss.Title).appendTo(item);
            if (ss.deleable) {
                $('<a href="#"></a>').addClass('ssdel').appendTo(item).on('click', function (e) {
                    if (!item.is('.ssitem-del')) {
                        item.addClass('ssitem-del');
                        $.ajax({
                            url: '/Plugins/SearchScheme',
                            data: {
                                method: 'del',
                                pageCode: location.pathname.toUpperCase(),
                                id: item.data().Id
                            }, dataType: 'json'
                        })
                            .done(function (data) {
                                if (data === true) {
                                    item.remove();
                                    if (list.find('>*').length == 0) {
                                        list.hide();
                                        empty.show();
                                    }
                                }
                            });
                    }
                    e.stopImmediatePropagation();
                });
            }
        }

        function loadScheme() {
            $.ajax({
                url: '/Plugins/SearchScheme',
                data: {
                    pageCode: location.pathname.toUpperCase()
                }, dataType: 'json'
            })
                .done(function (data) {
                    if (data.length == 0) {
                        empty.show();
                        list.hide();
                    } else {
                        empty.hide();
                        list.empty().show();
                        $.each(data, function () {
                            addScheme(this);
                        });
                    }
                });
        }

        setTimeout(loadScheme, 100);

        //addScheme({
        //    id: '123',
        //    title: '无条件',
        //    deleable: true,
        //    data: {
        //        DealerCode: ''
        //    }
        //})
    }
});