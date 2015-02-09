using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using MyLib.MyBase.MyAjax;
using MyLib.MyBase.MyWeb;

namespace MyAjax
{
    public class MyAjaxHander : MyASHX
    {
        private Dictionary<string, Type> ClassList;

        /// <summary>
        /// Lấy tất cả các class thuộc một namespace
        /// </summary>
        private void GetAllClass(string Namespace)
        {
            try
            {
                //Lấy tất cả namespace
                Assembly mAssem = Assembly.Load(Namespace);

                //Tìm namesapce cần lấy các class
                foreach (Type mType in mAssem.GetTypes())
                {
                    ClassList.Add(mType.FullName.ToLower(), mType);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override void WriteHTML()
        {
            try
            {
                if (MyContext.Application["AjaxClassList"] == null)
                {
                    //Lấy tất cả các Type của class dành cho Ajax
                    ClassList = new Dictionary<string, Type>();
                    GetAllClass("MyAjax");
                    MyContext.Application["AjaxClassList"] = ClassList;
                }
                else
                {
                    ClassList = (Dictionary<string, Type>)MyContext.Application["AjaxClassList"];
                }

                string NamePage = Request.Url.Segments[Request.Url.Segments.Length - 1];
                NamePage = NamePage.Replace(".ajax", "");
                string[] Arr = NamePage.Split('.');
                string ClassName = string.Empty;
                string MethodName = string.Empty;


                for (int i = 0; i < Arr.Length; i ++ )
                {
                    if(i== Arr.Length-1)
                    {
                        MethodName = Arr[i];
                    }
                    else
                    {
                        if(i>0)
                        {
                            ClassName += "." + Arr[i];
                        }
                        else
                        {
                            ClassName += Arr[i];
                        }
                        
                    }
                }                   

                //Lấy class
                if (ClassList.ContainsKey(ClassName.ToLower()))
                {
                    Type CurrentType;
                    ClassList.TryGetValue(ClassName.ToLower(), out CurrentType);

                    var CurrentClass = (MyAjaxBase)Activator.CreateInstance(CurrentType);
                    CurrentClass.MyContext = MyContext;

                    CurrentClass.RunMethod(MethodName);
                }
            }
            catch (Exception ex)
            {
                mLog.Error(ex);
                MyContext.Response.Write("Có lỗi xảy ra trong quá trình tải dữ liệu, xin vui lòng thử lại!");
            }
        }
    }
}
