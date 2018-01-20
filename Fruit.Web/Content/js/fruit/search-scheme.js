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