using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using MES.Models;
using MES.Models.ViewModel;

/// <summary>
/// 使用者資訊類別
/// </summary>
public static class UserAccount
{
    /// <summary>
    /// 登入使用者角色
    /// </summary>
    public static EnumList.LoginRole Role { get; set; } = EnumList.LoginRole.Guest;
    /// <summary>
    /// 登入使用者角色名稱
    /// </summary>
    public static string RoleName { get { return EnumList.GetRoleName(Role); } }
    /// <summary>
    /// 使用者帳號
    /// </summary>
    public static string Account { get; set; } = "";
    /// <summary>
    /// 使用者名稱
    /// </summary>
    public static string UserName { get; set; } = "";
    /// <summary>
    /// 使用者電子信箱
    /// </summary>
    public static string UserEmail { get; set; } = "";
    /// <summary>
    /// 使用者是否已登入
    /// </summary>
    public static bool IsLogin { get; set; } = false;


    /// <summary>
    /// 暫存編號
    /// </summary>
    public static string TempNo { get; set; } = "";  


    public static void Login()
    {
        using (MESEntities db = new MESEntities())
        {
            var data = db.users.Where(m => m.m_account == Account).FirstOrDefault();

            IsLogin = true;
            UserName = data.m_account;
            UserEmail = data.email_user;
            Role = EnumList.GetRoleType(data.role_no);

        }
    }

    public static void Logout()
    {

        Role = EnumList.LoginRole.Guest;
        Account = "";
        UserName = "";
        UserEmail = "";
        UserAccount.UploadImageMode = false;
        IsLogin = false;
    }



    public static bool UploadImageMode
    {
        get
        {
            bool bln_value = false;
            if (HttpContext.Current.Session["UploadImage"] != null)
            {
                string str_value = HttpContext.Current.Session["UploadImage"].ToString();
                bln_value = (str_value == "1");
            }
            return bln_value;
        }
        set
        { HttpContext.Current.Session["UploadImage"] = (value) ? "1" : "0"; }
    }

    //public static string UserImageUrl
    //{
    //    get
    //    {
    //        string str_url = "~/Images/user/guest.jpg";
    //        string str_file = string.Format("~/Images/user/{0}.jpg", Account);
    //        if (File.Exists(HttpContext.Current.Server.MapPath(str_file))) str_url = str_file;
    //        str_url += string.Format("?{0}", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
    //        return str_url;
    //    }
    //}


    public static string UserImageUrl
    {
        get
        {
            return UploadImageUrl(Account);
        }
    }

    public static string UploadImageUrl(string accountNo)
    {
        string str_url = "~/Images/user/guest.jpg";
        string str_file = string.Format("~/Images/user/{0}.jpg", accountNo);
        if (File.Exists(HttpContext.Current.Server.MapPath(str_file))) str_url = str_file;
        str_url += string.Format("?{0}", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        return str_url;

    }




}
