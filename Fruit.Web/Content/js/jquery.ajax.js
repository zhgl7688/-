var ajax_ip = "";
(function ($) { 
    $.extend({ 
        api: {
            ajax: function (postData, fw, fn) {  
                var urls = ajax_ip + "/Account/" + fw.method;
                console.log(urls);
                $.ajax({
                    type: 'post',
                    dataType: "json",
                    url: urls,
                    data: postData,
                    cache: false,
                    async: false,
                    success: function (data) {
                        fn(data);
                    },
                    error: function (data) {
                        console.log("error:" + JSON.stringify(data));
                    }
                });
            },
            ajax_mvc: function (postData, fw, fn) { 
                var urls = ajax_ip + "/Account/" + fw.method;
                console.log(urls);
                var time = 0;
                window.setInterval(function () {
                    time += 100;
                }, 100);
                try {
                    $.ajax({
                        type: 'post',
                        dataType: "json",
                        url: urls,
                        data: postData,
                        cache: false,
                        async: false,
                        timeout: 30000,
                        success: function (data) {
                            console.log("本次请求耗时：" + time + "毫秒");
                            window.clearInterval();
                            var str = JSON.stringify(data);
                            str = str.replace(/null/g, '""');
                            str = str.replace('""""', '""');
                            data = JSON.parse(str);
                            console.log(str)
                            fn(data);
                        },
                        error: function (data) {
                         
                            console.log("error:" + JSON.stringify(data));
                        }
                    });
                } catch (e) {
                    console.log("请求ajax发生异常！");
                }
            },
          
        }

    }); 
}(jQuery));

