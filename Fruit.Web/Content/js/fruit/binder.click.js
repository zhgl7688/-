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