using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyLib.MyBase.MyWeb;
using MyLoad.LoadStatic;
using MyLeu.Setting;
using MyLoad.LoadMember;
using MyLoad.LoadMember;
namespace LeuLeu.Page
{
    /// <summary>
    /// Summary description for Login
    /// </summary>
    public class Login : MyASHX
    {
        public override void WriteHTML()
        {
            try
            {
                LLogin mLogin = new LLogin(this);
                Write(mLogin.GetHTML());
            }
            catch (Exception ex)
            {
               
            }
        }
    }
}