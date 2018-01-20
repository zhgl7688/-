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