using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyLib.MyBase.MyWeb;
using MyLoad.LoadStatic;
using MyLeu.Setting;
using MyLoad.LoadContent;
namespace LeuLeu.Page
{
    /// <summary>
    /// Summary description for Detail
    /// </summary>
    public class Detail : MyASHX
    {
        public override void WriteHTML()
        {
            try
            {
                this.PageCode = WebSetting.ListPage.Home.ToString();

                LDetail mDetail = new LDetail(this);
                string strDetail = mDetail.GetHTML();

                LHeader mHeader = new LHeader(this);
                mHeader.mHeaderObj.Title = mDetail.mNewsObj.Title;
                mHeader.mHeaderObj.Image = mDetail.mNewsObj.Image;
                mHeader.mHeaderObj.Description = WebSetting.Description;
                mHeader.mHeaderObj.Link = mDetail.mNewsObj.DetailLink();

                Write(mHeader.GetHTML());

                LFaceBook mFaceBook = new LFaceBook(this);
                Write(mFaceBook.GetHTML());

                LBanner mBanner = new LBanner(this);
                Write(mBanner.GetHTML());

                Write(strDetail);

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