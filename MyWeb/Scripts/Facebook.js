window.fbAsyncInit = function ()
{
    FB.init({
        appId: '976339585715527',
        xfbml: true,
        version: 'v2.1'
    });

    // Sự kiện khi click Like or Remove Like
    FB.Event.subscribe('edge.create', function ()
    {

    });
    FB.Event.subscribe('edge.remove', function ()
    {

    });

};

(function (d, s, id)
{
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));
