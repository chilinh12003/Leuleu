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

namespace MyAjax.AjaxNews
{
    public class MyAjaxNews : MyAjaxBase
    {

        /// <summary>
        /// Đồng bộ thông tin report của 1 tin bài
        /// </summary>
        public void SyncCount()
        {
            try
            {
               
              
                string url = string.Empty;
                GetParemeter<string>(ref url, "url");

                if (string.IsNullOrEmpty(url))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "url không hợp lệ"));
                    return;
                }
                //url có dạng http://leuleu.vn/chi-tiet/thanh-nien-cung-cua-nam-10.html
                url = url.Replace("http://leuleu.vn/chi-tiet/", "").Replace(".html", "");

                string[] arr = url.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                string strID = arr[arr.Length - 1];

                int NewsID = 0;
                if(!int.TryParse(strID,out NewsID))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "url không hợp lệ"));
                    return;
                }
                NewsObject mNewsObj = new NewsObject();
                News mNews = new News();
                DataTable mTable = mNews.Select(1, NewsID.ToString());

                if(mTable.Rows.Count < 1)
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Fail, "url không hợp lệ"));
                    return;
                }
                List<NewsObject> mList = new List<NewsObject>();

                mList = MyConvert.ConvertTable2Object<NewsObject>(mTable);

                mNewsObj = mList[0];

                //Cập nhật thông tin từ facebook xuong db               
                if (mNewsObj.SyncFacebookInfo(mNews))
                {
                    ListAjaxResult.Add(new AjaxResult("Result", (int)AjaxResult.TypeResult.Success, "Đồng bộ thành công"));
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
    }
}
