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