/* 首页功能脚本 */
$(function () {

    // 这里修改默认不可关闭首页的图标，标题与地址
    var homePage = {
        iconCls: 'icon-house',
        title: '我的桌面',
        url:'/Home/About'
    }

    function navMenuClick(e) {
        var li = $(e.target);
        if (!li.is('li')) {
            li = li.closest('li');
        }
        var menu = li.data('menu');
        showDocTab(menu.IconClass, menu.MenuName, menu.URL, menu.MenuCode);
    }

    function rebuildNavMenuItem(ul, data, parentCode) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].ParentCode == parentCode) {
                var li = $('<li></li>').data('menu', data[i]).appendTo(ul).on('click', navMenuClick);
                var a = $('<a></a>').attr('href', 'javascript:void(0);').appendTo(li);
                var icon = $('<span class="icon ' + data[i].IconClass + '">&nbsp;</span>').appendTo(a);
                $('<span class="nav"></span>').text(data[i].MenuName).appendTo(a);
            }
        }
    }

    var restoreMode = false;

    function rebuildNavMenu(data) {
        var first = true;
        var menu = null;
        for (var i = 0; i < data.length; i++) {
            if (data[i].ParentCode == '0' || data[i].ParentCode == '') {
                var navMenuTree = $('<ul></ul>')
                $('#navMenu').accordion('add', {
                    title: data[i].MenuName,
                    iconCls:data[i].IconClass,
                    content: navMenuTree,
                    selected: first
                });
                first = false;
                rebuildNavMenuItem(navMenuTree, data, data[i].MenuCode);
            }
            if (data[i].URL && data[i].URL.length > 1 && '#' + data[i].URL == location.hash) {
                menu = data[i];
            }
        }

        restoreMode = true;
        showDocTab(homePage.iconCls, homePage.title, homePage.url, '==HOME==');
        var DOC_TAB_LAYOUT = fruit.cookie('DOC_TAB_LAYOUT');
        var selectedTabIndex = -1;
        if (DOC_TAB_LAYOUT) {
            try {
                var tabs = JSON.parse(DOC_TAB_LAYOUT);
                $.each(tabs.tabs, function (index) {
                    if ('#' + this.url == location.hash) {
                        selectedTabIndex = index;
                    }
                    showDocTab(this.iconCls, this.title, this.url, this.code);
                });
            } catch (e) { }
        }
        restoreMode = false;

        //console.log('selectedTabIndex', selectedTabIndex);

        if (selectedTabIndex > -1) {
            $('#mainTabs').tabs('select', selectedTabIndex);
        } else {
            var tabs = $('#mainTabs').tabs('tabs');
            if (tabs.length > 0) {
                $('#mainTabs').tabs('select', selectedTabIndex = tabs.length - 1);
            }
        }

        if (selectedTabIndex > -1) {
            $('#mainTabs').tabs('options').onSelect(null, selectedTabIndex);
        }
    }

    // 保存主选项卡的当前布局状态
    function saveDocTabLayout() {
        if (restoreMode) return;
        var data = { tabs: [] };
        var tabs = $('#mainTabs').tabs('tabs');
        $.each(tabs, function (i) {
            if (i == 0) return; // 不保存默认首页信息
            var panelOpt = $(this).data().panel.options;
            data.tabs.push({
                title: panelOpt.title,
                iconCls: panelOpt.iconCls,
                code: $(this).find('iframe').data('code'),
                url: $(this).find('iframe').data('src')
            });
        });
        fruit.cookie('DOC_TAB_LAYOUT', JSON.stringify(data));
    }

    $('#mainTabs').tabs({
        'tools': '#mainTabsTools',
        'fit': true,
        'scroll': 'no',
        'onSelect': function (title, index) {
            if (restoreMode) return;
            var tab = $('#mainTabs').tabs('getTab', index);
            var iframe = tab.find('iframe');
            location.hash = '#' + iframe.data('src');
            if (!iframe.attr('src')) {
                iframe.attr('src', iframe.data('src'));
            }
            //console.log('onSelect', tab.find('iframe').data('src'));
        },
        'onAdd': function (title, index) {
            saveDocTabLayout();
        },
        'onClose': function (title, index) {
            saveDocTabLayout();
        }
    });

    $('#btnCloseAllWindow').on('click', function () {
        if (confirm('确定要关闭所有窗口吗？')) {
            var count = $('#mainTabs').tabs('tabs').length;
            while (count > 1) {
                count--;
                $('#mainTabs').tabs('close', 1);
            }
        }
    });

    $('#btnFullscreen').on('click', function () {
        var panels = $('body').data().layout.panels;
        if ($(this).find('.icon-screen_actual').length) {
            panels.north.panel('open');
            panels.west.panel('open');
            $(this).find('.icon-screen_actual').removeClass('icon-screen_actual').addClass('icon-screen_full');
        } else {
            panels.north.panel('close');
            panels.west.panel('close');
            $(this).find('.icon-screen_full').removeClass('icon-screen_actual').addClass('icon-screen_actual');
        }
        $('body').layout('resize');
    });

    $('#btnRefresh').on('click', function () {
        var tab = $('#mainTabs').tabs('getSelected');
        var iframe = $('iframe', tab);
        if (iframe.length > 0) {
            iframe[0].contentWindow.location = iframe.attr('src');
        }
    });

    fruit.ajax({
        url: 'api/menu',
        success: rebuildNavMenu
    });

    $('.changepwd').on('click', function () {
        com.dialog({
            title: "&nbsp;修改密码",
            iconCls: 'icon-key',
            width: 320,
            height: 204,
            html: "#password-template",
            viewModel: function (w) {
                //w.find("[name=UserCode]").val(@ViewBag.UserName);
                w.find("#pwd_confirm").click(function () { w.dialog('close'); });
                w.find("#pwd_close").click(function () { w.dialog('close'); });
            }
        });
    });
    $('.loginOut').on('click', function () {
        $.messager.confirm('系统提示', '您确定要退出本次登录吗?', function (r) {
            if (r) location.href = '/Account/Login/Logout';
        });
    });

});

/**
 * 在主选项卡上添加或选择一个文档
 */
function showDocTab(iconCls, title, url, code) {
    $ = parent.$ || $;
    var tabIndex = $('#mainTabs').tabs('tabs').indexOf(function (tab) { return $(tab).find('iframe').data('src') == url; });
    if (tabIndex > -1) {
        $('#mainTabs').tabs('select', tabIndex);
        var frame = $('#mainTabs').tabs('getTab', tabIndex).find('iframe');
        frame[0].contentWindow.location.reload();
        return;
    }

    function addNewDocTab() {

        var newFrame = $('<iframe style="width:100%;height:100%;" frameborder="0" scrolling="auto" data-code="' + code + '"></iframe>');
        newFrame.data('src', url);
        $('#mainTabs').tabs('add', {
            iconCls: iconCls,
            title: title,
            content: newFrame,
            closable: code != '==HOME=='
        });
    }

    if ($('#mainTabs').tabs('tabs').length >= 10) {
        $.messager.confirm('警告', '您当前打开了太多的页面，如果继续打开，会造成程序运行缓慢，无法流畅操作！', function (r) {
            if (r) { addNewDocTab(); }
        });
        return;
    } else {
        addNewDocTab();
    }
}

/**
 * 使用 iframe 做参数删除一个选项卡
 */
function closeDocTab(frame) {
    var tabIndex = $('#mainTabs').tabs('tabs').indexOf(function (tab) { return $(tab).find('iframe')[0] == frame });
    $('#mainTabs').tabs('close', tabIndex);
}
function changeDocTab(frame, url) {
    $(frame).data('src', url).attr('src', url);
    location.hash = '#' + url;
}