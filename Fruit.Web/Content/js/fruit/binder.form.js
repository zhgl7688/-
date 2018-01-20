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