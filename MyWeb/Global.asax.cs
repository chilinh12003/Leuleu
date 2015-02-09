using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using MyLeu.Setting;
using MyLib.MyUtility.UploadFile;
namespace LeuLeu
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            //nếu tồn tại file ảnh mà (ảnh đã upload mà bài chưa đăng thì xóa đi
            if(string.IsNullOrEmpty(WebSetting.LastImageUpload))
            {
                MyUploadImage mUpload = new MyUploadImage("_Mem");
                mUpload.DeleteOldFile(WebSetting.LastImageUpload);
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}