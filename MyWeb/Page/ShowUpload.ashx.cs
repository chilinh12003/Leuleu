using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyLib.MyBase.MyWeb;
using MyLoad.LoadStatic;
using MyLeu.Setting;
using MyLoad.LoadMember;
namespace LeuLeu.Page
{
    /// <summary>
    /// Summary description for ShowUpload
    /// </summary>
    public class ShowUpload : MyASHX
    {
        public override void WriteHTML()
        {
            try
            {
                if (string.IsNullOrEmpty(Request.QueryString["tid"]))
                {
                    LShowImageUpload mShowUpload = new LShowImageUpload(this);
                    Write(mShowUpload.GetHTML());
                }
                else
                {
                    LShowVideoUpload mShowUpload = new LShowVideoUpload(this);
                    Write(mShowUpload.GetHTML());
                }

            }
            catch (Exception ex)
            {
                mLog.Error(ex);
            }
        }
    }
}