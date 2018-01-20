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