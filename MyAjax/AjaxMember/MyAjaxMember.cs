using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLib.MyBase.MyAjax;
using MyLeu.Setting;
using MyLib.MyUtility;
using MyLeu.DB.Member;
using MyLib.MyUtility.UploadFile;
using System.Web;
using MyLeu.DB.Content;
using System.Data;
namespace MyAjax.AjaxMember
{
    public class MyAjaxMember : MyAjaxBase
    {

        public string CreateEmbed(ref NewsObject mNewsObj)
        {
            string YoutubeID = string.Empty;
            if(!string.IsNullOrEmpty(mNewsObj.Image))
            {
                //Là từ youtube
                YoutubeID = mNewsObj.Image.Substring(mNewsObj.Image.IndexOf("v=")+2, 11);
                mNewsObj.Embed = YoutubeID;
            }
            return YoutubeID;
        }
        /// <summary>
        /// Insert 1 tin bài
        /// </summary>
        public void PostNews()
        {
            try
            {
                //nếu chưa đăng nhập
                if (WebSetting.MemberLogined.IsNull)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "Xin vui lòng đăng nhập trước"));
                    return;
                }
                NewsObject mNewsObj = new NewsObject();             

                GetParemeter<int>(ref mNewsObj.CateID, "CateID");
                GetParemeter<string>(ref mNewsObj.Image, "Image");
                GetParemeter<string>(ref mNewsObj.Title, "Title");
                GetParemeter<string>(ref mNewsObj.ResourceLink, "ResourceLink");  
   
              
                if(string.IsNullOrEmpty( mNewsObj.Image))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "Xin vui lòng chọn file ảnh cần tải lên."));
                    return;
                }
                if (string.IsNullOrEmpty(mNewsObj.Title))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "Xin vui lòng nhập tiêu đề của bài viết."));
                    return;
                }
                if (mNewsObj.Title.Length > 120)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "Tiêu đề bài viết không được vượt quá 120 ký tự."));
                    return;
                }

                if (!MyFile.CheckExistURL(mNewsObj.Image))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "File ảnh không tồn tại, xin vui lòng kiểm tra lại"));
                    return;
                }

                mNewsObj.Title = MyText.RemoveScript(mNewsObj.Title);
                mNewsObj.ResourceLink = MyText.RemoveScript(mNewsObj.ResourceLink);


                mNewsObj.CreateDate = DateTime.Now;
                mNewsObj.ModifyDate = DateTime.Now;
                mNewsObj.UpdateDate = DateTime.Now;
                mNewsObj.MemberID = WebSetting.MemberLogined.MemberID;
                mNewsObj.OpenID = WebSetting.MemberLogined.OpenID;
                mNewsObj.MemberName = WebSetting.MemberLogined.MemberName;
                mNewsObj.StatusID = (int)News.Status.Active;
                mNewsObj.TypeID = (int)News.NewsType.Image;

                List<NewsObject> mList = new List<NewsObject>();
                mList.Add(mNewsObj);

                DataTable mTable = MyConvert.ConvertObject2Table<NewsObject>(mList);
                DataSet mSet = new DataSet("Parent");
                mTable.TableName = "Child";
                mSet.Tables.Add(mTable);

                
                MyConvert.ConvertDateColumnToStringColumn(ref mSet);

                News mNews = new News();
                DataTable mTable_Return = mNews.InsertReturn(1, mSet.GetXml());

                if(mTable_Return.Rows.Count > 0)
                {
                    int.TryParse(mTable_Return.Rows[0][0].ToString(), out mNewsObj.NewsID);
                    WebSetting.LastImageUpload = string.Empty;
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, mNewsObj.DetailLink(), "Đăng bài thành công"));
                }
                else
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "Thất bại"));
                }

            }
            catch (Exception ex)
            {
                mLog.Error(ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, MyAjaxMessage.CommonError.Error));
            }
            finally
            {
                MyContext.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }
        public void PostVideoNews()
        {
            try
            {
                //nếu chưa đăng nhập
                if (WebSetting.MemberLogined.IsNull)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "Xin vui lòng đăng nhập trước"));
                    return;
                }
                NewsObject mNewsObj = new NewsObject();

                GetParemeter<int>(ref mNewsObj.CateID, "CateID");
                GetParemeter<string>(ref mNewsObj.Image, "Image");
                GetParemeter<string>(ref mNewsObj.Title, "Title");
                GetParemeter<string>(ref mNewsObj.ResourceLink, "ResourceLink");

               
                if (string.IsNullOrEmpty(mNewsObj.Image))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "Xin vui nhập link Youtube."));
                    return;
                }
                if (string.IsNullOrEmpty(mNewsObj.Title))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "Xin vui lòng nhập tiêu đề của bài viết."));
                    return;
                }
                if (mNewsObj.Title.Length > 120)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "Tiêu đề bài viết không được vượt quá 120 ký tự."));
                    return;
                }
                string YoutubeID = CreateEmbed(ref mNewsObj);

                if(string.IsNullOrEmpty(YoutubeID))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "Định đạng link không đúng, xin vui lòng kiểm tra lại"));
                    return;
                }
                mNewsObj.Image = "http://img.youtube.com/vi/"+YoutubeID+"/0.jpg";

                if (!MyFile.CheckExistURL(mNewsObj.Image))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "Link không tồn tại, xin vui lòng kiểm tra lại"));
                    return;
                }

                mNewsObj.Title = MyText.RemoveScript(mNewsObj.Title);
                mNewsObj.ResourceLink = MyText.RemoveScript(mNewsObj.ResourceLink);


                mNewsObj.CreateDate = DateTime.Now;
                mNewsObj.ModifyDate = DateTime.Now;
                mNewsObj.UpdateDate = DateTime.Now;
                mNewsObj.MemberID = WebSetting.MemberLogined.MemberID;
                mNewsObj.OpenID = WebSetting.MemberLogined.OpenID;
                mNewsObj.MemberName = WebSetting.MemberLogined.MemberName;
                mNewsObj.StatusID = (int)News.Status.Active;
                mNewsObj.TypeID = (int)News.NewsType.Video;

                List<NewsObject> mList = new List<NewsObject>();
                mList.Add(mNewsObj);

                DataTable mTable = MyConvert.ConvertObject2Table<NewsObject>(mList);
                DataSet mSet = new DataSet("Parent");
                mTable.TableName = "Child";
                mSet.Tables.Add(mTable);


                MyConvert.ConvertDateColumnToStringColumn(ref mSet);

                News mNews = new News();
                DataTable mTable_Return = mNews.InsertReturn(1, mSet.GetXml());

                if (mTable_Return.Rows.Count > 0)
                {
                    int.TryParse(mTable_Return.Rows[0][0].ToString(), out mNewsObj.NewsID);
                    WebSetting.LastImageUpload = string.Empty;
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, mNewsObj.DetailLink(), "Đăng bài thành công"));
                }
                else
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "Thất bại"));
                }

            }
            catch (Exception ex)
            {
                mLog.Error(ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, MyAjaxMessage.CommonError.Error));
            }
            finally
            {
                MyContext.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }

        /// <summary>
        /// Upload hình ảnh lên server
        /// </summary>
        public void Upload()
        {
            try
            {
                //nếu chưa đăng nhập
                if(WebSetting.MemberLogined.IsNull)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "Xin vui lòng đăng nhập trước"));
                    return;
                }
                HttpPostedFile mPostedFile = null;
                if (MyContext.Request.Files.Count > 0)
                    mPostedFile = MyContext.Request.Files[0];
                
                MyUploadImage mUpload = new MyUploadImage(mPostedFile, "Mem_");

                if(mUpload.Upload())
                {
                    //Nếu là upload nhiều file ảnh nhiều lần lên thì chỉ giữ file ảnh cuối
                    if(!string.IsNullOrEmpty(WebSetting.LastImageUpload))
                    {
                        mUpload.DeleteOldFile(WebSetting.LastImageUpload);
                    }
                    WebSetting.LastImageUpload = mUpload.UploadedPhysicalPathFile;
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, mUpload.UploadedPathFile, mUpload.Message));
                }
                else
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, mUpload.Message));
                }

            }
            catch (Exception ex)
            {
                mLog.Error(ex);
                ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Error, MyAjaxMessage.CommonError.Error));
            }
            finally
            {
                MyContext.Response.Write(MyJSON.ToJSON(ListAjaxResult.ToArray()));
            }
        }

    }
}
