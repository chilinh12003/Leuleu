/// <reference path="jquery.js" />
/// <reference path="ForAll.js" />
/// <reference path="loading.js" />

(function ($)
{
    $.fn.faceBox = function (opt)
    {
        options = $.extend($.fn.faceBox.settings, opt);

        return this.each(function ()
        {
            $.fn.faceBox.init(options);
        });
    };

    $.fn.faceBox.settings = {
        faceBoxID: "#faceBox",
        overID: "#fbOver",
        contentID: "#fbContent",
        closeID: "#fbClose", //ID của nút đống facebox
        fbWidth: 0,
        fixWidh: false, //Nếu = true: width của facebox không thay đổi khi windows resize
        fixHeight: false, //Nếu = true: Height của facebox không thay đổi khi windows resize
        showloading: false, //cho phep showload dinh hay khong
        maxTimeoutShow: 1000, //Nếu như facebox load vượt quá thời gian này thì facebox sẽ show. nếu =0 thì sẽ show ngay
        callBack: function () { }, //function se dc goi sau khi tat ca moi thu da ket thuc
        ajaxCallBack: function () { }, //function se duoc goi sau khi noi dung cua Ajax duoc add vao div content
        okCallBack: function () { } //function sẽ được gọi vưới trường hợp thông báo mà có nút OK
    };

    //Đếm số lần các đối tượng đang load và chưa finish.
    var imgCount = 0;
    var delayTimeout = null;

    //Cho biet facebox da hien thi len hay chua
    var isShow = false;

    var options = $.fn.faceBox.settings;

    $.fn.faceBox.getOptions = function ()
    {
        return options;
    };

    $.fn.faceBox.setOptions = function (opt)
    {
        options = opt;
    };

    //Khởi tạo và trả về ctr div facebox
    $.fn.faceBox.init = function (opt)
    {
        var ctr_fb = $(opt.faceBoxID).get(0);
        if (ctr_fb)
        {
            return ctr_fb;
        }

        $("body").append('  <div class="face-box" id="' + opt.faceBoxID.replace("#", "") + '">\
                                <div class="fb-transfer">\
                                </div>\
                                <div class="fb-over" id="' + opt.overID.replace("#", "") + '">\
                                    <div class="fb-content" id="' + opt.contentID.replace("#", "") + '"></div>\
                                </div>\
                            </div>');

        ctr_fb = $(opt.faceBoxID).get(0);
        if (ctr_fb)
        {
            $(".over").click(function (e)
            {
                if (e == null) { e = window.event; }

                var sender = e.target;
                if (sender != this)
                    return;

                $.fn.faceBox.close();
            });


            $(window).resize(function ()
            {
                $.fn.faceBox.reshow();
            });
            return ctr_fb;
        }
        else
        {
            $.error('Khong tim thay div facebox');
        }
    };

    $.fn.faceBox.close = function ()
    {
        $(options.faceBoxID).css("display", "none");
        $(options.contentID).html("");
        isShow = false;
        imgCount = 0;
        delayTimeout = null;
        $(window).resize(function ()
        {

        });
    };
    $.fn.faceBox.setPositonContent = function ()
    {
        $(options.faceBoxID).css({ visibility: "hidden", display: "block" });

        var $ctr_content = $(options.contentID);
        $ctr_content.css({ width: "auto", height: "auto" });

        if (options.fbWidth > 0)
            $ctr_content.css({ width: options.fbWidth + "px" });
        else
        {
            $ctr_content.css({ width: "auto" });
        }

        var w = $ctr_content.outerWidth();
        var h = $ctr_content.outerHeight();

        var s_w = $(window).width();
        var s_h = $(window).height();

        if (w >= s_w && !options.fixWidh)
        {
            w = s_w;
            $ctr_content.css({ width: w + "px" });
        }
        if (h >= s_h && !options.fixHeight)
        {
            h = s_h;
            $ctr_content.css({ height: h + "px" });
        }

        var top = (s_h - h) / 2;
        var left = (s_w - w) / 2;

        if (top < 0)
            top = 0;

        if (left < 0)
            left = 0;

        $ctr_content.css({ top: top + "px", left: left + "px" });
        $(options.faceBoxID).css({ visibility: "", display: "none" });

    };

    $.fn.faceBox.show = function ()
    {
        if (options.showloading)
        {
            //HideLoading();
        }
        if (isShow)
            return;

        $.fn.faceBox.setPositonContent();
        $(options.faceBoxID).css({ display: "block" });
        isShow = true;
        clearTimeout(delayTimeout);
    };

    //Khi thích thước của facebox thay đổi thì cần thiếp lập lại tọa độ cho facebox
    $.fn.faceBox.reshow = function ()
    {
        if (isShow != true)
            return;

        $.fn.faceBox.setPositonContent();
        $(options.faceBoxID).css({ display: "block" });
        isShow = true;
        clearTimeout(delayTimeout);

    };

    $.fn.faceBox.reshowDelay = function ()
    {
        var $ctr_content = $(options.contentID);
        $ctr_content.css({ width: "auto", height: "auto" });

        if (options.fbWidth > 0)
            $ctr_content.css({ width: options.fbWidth + "px" });
        else
        {
            $ctr_content.css({ width: "auto" });
        }

        var w = $ctr_content.outerWidth();
        var h = $ctr_content.outerHeight();
        var s_w = $(window).width();
        var s_h = $(window).height();

        if (w >= s_w && !options.fixWidh)
        {
            w = s_w;
            $ctr_content.css({ width: w + "px" });
        }
        if (h >= s_h && !options.fixHeight)
        {
            h = s_h;
            $ctr_content.css({ height: h + "px" });
        }

        var top = (s_h - h) / 2;
        var left = (s_w - w) / 2;

        if (top < 0)
            top = 0;

        if (left < 0)
            left = 0;
        $ctr_content.animate({ top: top + "px", left: left + "px" }, 500, function ()
        {

        });
    };

    //Kiểm tra xem còn tag nào đang load không, nếu đã load xong hết thì show facebox
    var checkLoadComplete = function ()
    {
        if ($(options.contentID).find("img").length < 1)
        {
            $.fn.faceBox.show();
            return;
        }

        delayTimeout = setTimeout(' $.fn.faceBox.show();', options.maxTimeoutShow);       

        $(options.contentID).find("img").each(function ()
        {
            imgCount++;
            $(this).load(function ()
            {
                imgCount--;
                if (imgCount <= 0)
                {
                    $.fn.faceBox.show();
                }
            });
        });
    };

    //Thiết lập 1 control nào đó là nút Close Facebox thì gắn function click để close fb
    var setCloseControl = function ()
    {
        $(options.faceBoxID).find(options.closeID).each(function ()
        {
            $(this).click(function ()
            {
                $.fn.faceBox.close();
            });
        });
    };

    var setHTML = function (strHTML)
    {
        var ctr_content = $(options.contentID);
        $(ctr_content).append(' <div class="fb-header">\
                                    <a class="fb-close-btn" id="' + options.closeID.replace("#", "") + '" href="javascript:void(0);"></a>\
                                </div>');
        $(ctr_content).append(strHTML);

        try
        {
            options.ajaxCallBack();
        }
        catch (e)
        {
            alert(e);
        }
        checkLoadComplete();
        setCloseControl();
        if (imgCount <= 0)
        {
            $.fn.faceBox.show();
        }

        options.callBack();
    };

    //Show một chuỗi HTML lên facebox
    $.fn.faceBox.showHTML = function (strHTML)
    {
        try
        {
            $.fn.faceBox.close();
            setHTML(strHTML);
        }
        catch (e)
        {
            alert(e);
        }
    };

    $.fn.faceBox.showMessage = function (strMessage)
    {
        try
        {
            $.fn.faceBox.close();
            var strHTML = ' <div class="fb-message"><div class="fb-content">\
                        <div class="fb-title">' + strMessage + '</div>\
                        <div class="fb-button"><a class="btn-grey" href="javascript:void(0);" id="closeFaceBox">Hủy bỏ</a></div>\
                    </div></div>';
            setHTML(strHTML);
        }
        catch (e)
        {
            alert(e);
        }
    };


    //Hiển thị facebox với nội dung lấy từ Ajax
    $.fn.faceBox.showURL = function (url)
    {
        try
        {
            if (options.showloading)
            {
                //ShowLoading();
            }
            $.ajax({
                type: "POST", //Phương thức gửi request là POST hoặc GET
                url: url, //Đường dẫn tới nơi xử lý request ajax
                success: function (htmlContent)
                {
                    $.fn.faceBox.close();

                    setHTML(htmlContent);

                }
            });
        }
        catch (e)
        {
            alert(e);
        }
    };


    //Hiển thị facebox với nội dung lấy từ Ajax
    $.fn.faceBox.showAjax = function (url, para)
    {
        try
        {
            if (options.showloading)
            {
                //ShowLoading();
            }
            $.ajax({
                type: "POST", //Phương thức gửi request là POST hoặc GET
                data: para,
                url: url, //Đường dẫn tới nơi xử lý request ajax
                success: function (string)
                {
                    $.fn.faceBox.close();
                    var arr_JSON = $.parseJSON(string);
                    $.each(arr_JSON, function ()
                    {
                        //nếu như biến là kết quả trả về
                        if (this.Parameter == "Result")
                        {
                            //nếu kiểu trả về là success
                            if (this.CurrentTypeResult == "1")
                            {
                                setHTML(this.Value);
                            }
                            else
                            {
                                //HideLoading();
                                alert(this.Description);
                            }
                        }
                    });
                }
            });
        }
        catch (e)
        {
            alert(e);
        }
    };

    $.fn.faceBox.showIFrame = function (URL, width, hegith, IsShowScroll)
    {
        $.fn.faceBox.close();
        if (IsShowScroll)
        {
            setHTML(' <iframe width="' + width + '" height="' + hegith + '" src="' + URL + '" id="ifr_FaceBox" frameborder="0" scrolling="auto" onload="");"></iframe>');
        }
        else
        {
            setHTML(' <iframe width="' + width + '" height="' + hegith + '" src="' + URL + '" id="ifr_FaceBox" frameborder="0" scrolling="no" onload=""></iframe>');
        }
    }
})(jQuery);