var url = Domain + "/MyAjax.AjaxNews.MyAjaxNews.SyncCount.ajax";
$.ajax({
    type: "POST", //Phương thức gửi request là POST hoặc GET
    data: "TypeID=1&url=" + window.location.href,
    url: url, //Đường dẫn tới nơi xử lý request ajax
    success: function (string)
    {
        var arr_JSON = $.parseJSON(string);

        $.each(arr_JSON, function ()
        {
            //nếu như biến là kết quả trả về
            if (this.Parameter == "Result")
            {
                
            }
        });
    }
});