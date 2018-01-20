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