/// <reference path="../jquery-plugin/jquery-ui/js/jquery-ui-datepick.min.js" />
// 加载样式表
fruit.loadStylesheet = function (url, doc, reload, callback) {
    if (typeof doc == 'undefined') {
        doc = document;
    }
    var links = doc.getElementsByTagName("link");
    for (var i = 0; i < links.length; i++)
        if (links[i].href.indexOf(url) > -1) {
            if (reload)
                links[i].parentNode.removeChild(links[i])
            else
                return;
        }
    var container = doc.getElementsByTagName("head")[0];
    var css = doc.createElement("link");
    css.rel = "stylesheet";
    css.type = "text/css";
    css.media = "screen";
    css.href = url;
    if ($.isFunction(callback)) {
        css.onload = function () {
            callback(css);
        }
    }
    container.appendChild(css);
}

// 加载脚本文件
fruit.loadScript = function (url, doc, reload, callback) {
    if (typeof doc == 'undefined') {
        doc = document;
    }
    $.getScript(url, function () {
        if ($.isFunction(callback)) {
            callback({
                tagName: 'SCRIPT',
                src: url
            });
        }
    });
    //var links = doc.getElementsByTagName("link");
    //for (var i = 0; i < links.length; i++)
    //    if (links[i].href.indexOf(url) > -1) {
    //        if (reload)
    //            links[i].parentNode.removeChild(links[i])
    //        else
    //            return;
    //    }
    //var container = doc.getElementsByTagName("body")[0];
    //var js = doc.createElement("script");
    //js.type = "text/javascript";
    //js.async = true;
    //js.src = url;
    //if ($.isFunction(callback)) {
    //    js.onload = function () {
    //        callback(js);
    //    }
    //}
    //container.appendChild(js);
}

// 用于自动加载其他插件资源的加载器
fruit.loader = {
    // 已加载的插件
    installedPlugins: [],
    // 已加载的脚本
    loadedScripts: [],
    // 第三方插件信息，包括识别选择器(selector)，引用样式表(link)，引用脚本(js)和针对的自定义初始化方法(init)
    plugins: [
        {
            selector: '.easyui-daterange',
            link: [
                '/Content/js/jquery-plugin/jquery-ui/css/jquery-ui.css',
                '/Content/js/jquery-plugin/daterange/jquery.daterange.css'
            ],
            js: [
                '/Content/js/jquery-plugin/jquery-ui/js/jquery-ui-datepick.min.js',
                '/Content/js/jquery-plugin/daterange/jquery.daterange.js'
            ],
            init: function ($ele) {
                //console.log('daterange inited');
                return $ele.daterange();
            }
        }
    ],
    init: function (owner, callback) {
        setTimeout(function () {
            if ($.isFunction(callback)) {
                callback();
            }
        }, 1000)
    }
};