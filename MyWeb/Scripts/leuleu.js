/// <reference path="jquery-2.1.1.js" />
/// <reference path="facebox.js" />

var Domain = "http://leuleu.vn";
var FacebookAppID = "976339585715527";
var IsLogined = false;

var fn_InitFaceBook = function()
{

}
var fn_BeginLoad = function ()
{

}
var fn_DocumentClick = function (e)
{
}
//su kien click cua Documnet hien tai.
var fn_DocumentClick_Bak = function () { };
$(document).ready(function ()
{
    //Khoi tao facebox
    $(document).faceBox({ faceBoxID: "#div_fb" });
    //$(window).scroll(fixTitle); 

    fn_BeginLoad();

    $("#btnUpload-img").click(function ()
    {
        var fb_opt = $(this).faceBox.getOptions();
        fb_opt.fixHeight = true;
        fb_opt.fixWidh = true;
        fb_opt.maxTimeoutShow = 0;

        $(this).faceBox.setOptions(fb_opt);
        $(this).faceBox.showURL(Domain + "/Page/ShowUpload.ashx");

    });
    $("#btnUpload-video").click(function ()
    {
        var fb_opt = $(this).faceBox.getOptions();
        fb_opt.fixHeight = true;
        fb_opt.fixWidh = true;
        fb_opt.maxTimeoutShow = 0;

        $(this).faceBox.setOptions(fb_opt);
        $(this).faceBox.showURL(Domain + "/Page/ShowUpload.ashx?tid=1");

    });

    $(".popup-menu").hide();
    $(".popup-menu").removeClass("hide");

    $(".avatar").click(function ()
    {
        if ($(".popup-menu.user").is(':visible'))
        {
            $(".avatar").removeClass("selected");
            $(".popup-menu.user").hide('fast', function ()
            {
                fn_DocumentClick = fn_DocumentClick_Bak;
            });
        }
        else
        {
            $(".avatar").addClass("selected");

            //ẩn tất cả các popup đang hiện
            $(".popup-menu").hide('fast', function () { });

            $(".popup-menu.user").show('fast', function ()
            {
                fn_DocumentClick_Bak = fn_DocumentClick;
                fn_DocumentClick = function (e)
                {
                    if (e == null) { e = window.event; }

                    var sender = e.target;
                    if (sender === "undefined")
                        sender = e.srcElement;

                    if (sender && ($(sender).hasClass("avatar")))
                    {
                        return;
                    }
                    if ($(sender).parents(".avatar").size() > 0)
                    {
                        return;
                    }
                    $(".avatar").removeClass("selected");
                    $(".popup-menu.user").hide('fast', function ()
                    {
                        fn_DocumentClick = fn_DocumentClick_Bak;
                    });
                };
            });
        }
    });

    $(".upload-file").click(function ()
    {
        if ($(".popup-menu.upload").is(':visible'))
        {
            $(".avatar").removeClass("selected");
            $(".popup-menu.upload").hide('fast', function ()
            {
                fn_DocumentClick = fn_DocumentClick_Bak;
            });
        }
        else
        {
            $(".popup-menu").hide('fast', function () { });
            $(".popup-menu.upload").show('fast', function ()
            {
                fn_DocumentClick_Bak = fn_DocumentClick;
                fn_DocumentClick = function (e)
                {
                    if (e == null) { e = window.event; }

                    var sender = e.target;
                    if (sender === "undefined")
                        sender = e.srcElement;

                    if (sender && ($(sender).hasClass("upload-file")))
                    {
                        return;
                    }
                    if ($(sender).parents(".upload-file").size() > 0)
                    {
                        return;
                    }
                    $(".avatar").removeClass("selected");
                    $(".popup-menu.upload").hide('fast', function ()
                    {
                        fn_DocumentClick = fn_DocumentClick_Bak;
                    });
                };
            });
        }
    });

    $(document).click(function (e)
    {
        $(".popup-menu").hide
        fn_DocumentClick(e);
    });
});


///Fix title của mỗi tin khi scroll thanh cuộn
function fixTitle()
{
    $('article.list-item > header').each(function ()
    {
        var $this = $(this);
        var parent = $(this).parent('article').get(0);

        var parent_t = $(parent).offset().top - 47;
        var scrollTop = $(window).scrollTop();
        var this_h = $(this).outerHeight();

        var parent_h = $(parent).outerHeight();
        
        $(this).css({
            "width": $(this).outerWidth()+"px",
            "height": $(this).outerHeight() + "px",
        })

        if (scrollTop < 1)
        {
            $(this).css({
                "width": "100%",
                "height": "auto",
                "position": "initial",
                "top": "initial",
            });
            return;
        }
        if (scrollTop > parent_t)
        {
            if (scrollTop > (parent_t + parent_h - 20 - this_h))
            {
                $(this).css({
                    "width": $(this).outerWidth() + "px",
                    "height": $(this).outerHeight() + "px",
                    "position": "absolute",
                    "top": "initial",
                    "bottom": "23px",
                });
            }
            else
            {
                $(this).css({
                    "width": $(this).outerWidth() + "px",
                    "height": $(this).outerHeight() + "px",
                    "position": "fixed",
                    "top": "46px",
                });
            }
        }
        else
        {
            $(this).css({
                "width": "100%",
                "height": "auto",
                "position": "initial",
                "top": "initial",
            });
        }
    });
}
