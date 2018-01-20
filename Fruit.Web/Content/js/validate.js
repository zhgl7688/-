$.extend($.fn.validatebox.defaults.rules, {
    数字: { validator: function(value) { return /^[0-9]+\.{0,1}[0-9]{0,2}$/.test(value); }, message: '不是有效的数字'},
    电子邮件: { validator: function(value) { return /^\w+([\.\-]\w+)*\@\w+([\.\-]\w+)*\.\w+$/.test(value); }, message: '不是有效的电子邮件地址'},
    手机号码: { validator: function(value) { return /^((\(\d{3}\))|(\d{3}\-))?13\d{9}$/.test(value); }, message: '不是有效的手机号码'},
    身份证号: { validator: function(value) { return /^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{4}$/.test(value); }, message: '不是有效的身份证号'}
});
