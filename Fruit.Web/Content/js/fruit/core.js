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