using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyLib.MyBase.MyWeb;
using MyLoad.LoadStatic;
using MyLeu.Setting;
using MyLoad.LoadContent;
using MyLib.MyUtility;
namespace LeuLeu.Page
{
    /// <summary>
    /// Summary description for Home
    /// </summary>
    public class Home : MyASHX
    {

        public override void WriteHTML()
        {
            try
            {
                this.PageCode = WebSetting.ListPage.Home.ToString();

                LHeader mHeader = new LHeader(this);
                mHeader.mHeaderObj.Link = MyConfig.Domain;
                mHeader.mHeaderObj.Title = "Leuleu.vn - Hot";
                mHeader.mHeaderObj.Description =WebSetting.Description;
                mHeader.mHeaderObj.Image = MyConfig.Domain + "/img/Logo.png";
                
                Write(mHeader.GetHTML());

                LFaceBook mFaceBook = new LFaceBook(this);
                Write(mFaceBook.GetHTML());

                LBanner mBanner = new LBanner(this);
                Write(mBanner.GetHTML());

                LHot mHot = new LHot(this,true);
                Write(mHot.GetHTML());

                LFooter mFooter = new LFooter(this);
                Write(mFooter.GetHTML());
            }
            catch (Exception ex)
            {
                mLog.Error(ex);
            }
        }
    }
}