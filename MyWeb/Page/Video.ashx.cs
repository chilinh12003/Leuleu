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
    /// Summary description for Video
    /// </summary>
    public class Video : MyASHX
    {

        public override void WriteHTML()
        {
            try
            {
                this.PageCode = WebSetting.ListPage.Video.ToString();

                LHeader mHeader = new LHeader(this);
                mHeader.mHeaderObj.Link = MyConfig.Domain;
                mHeader.mHeaderObj.Title = "Leuleu.vn - Video";
                mHeader.mHeaderObj.Description = WebSetting.Description;
                mHeader.mHeaderObj.Image = MyConfig.Domain + "/img/Logo.png";

                Write(mHeader.GetHTML());

                LFaceBook mFaceBook = new LFaceBook(this);
                Write(mFaceBook.GetHTML());

                LBanner mBanner = new LBanner(this);
                Write(mBanner.GetHTML());

                LHot mHot = new LHot(this, true);
                mHot.mNewsType = MyLeu.DB.Content.News.NewsType.Video;
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