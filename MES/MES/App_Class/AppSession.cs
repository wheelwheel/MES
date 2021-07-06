using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


/// <summary>
/// Session 類別
/// </summary>
public class AppSession
{
    /// <summary>
    /// 表頭的目前主鍵值
    /// </summary>
    public static string MasterKeyValue
    {
        get { return (HttpContext.Current.Session[name: "MasterKeyValue"] == null) ? "" : HttpContext.Current.Session[name: "MasterKeyValue"].ToString(); }
        set { HttpContext.Current.Session[name: "MasterKeyValue"] = value; }
    }

    /// <summary>
    /// 明細的目前主鍵值
    /// </summary>
    public static string DetailKeyValue
    {
        get { return (HttpContext.Current.Session[name: "DetailKeyValue"] == null) ? "" : HttpContext.Current.Session[name: "DetailKeyValue"].ToString(); }
        set { HttpContext.Current.Session[name: "DetailKeyValue"] = value; }
    }

    /// <summary>
    /// 表頭的目前頁數
    /// </summary>
    public static int MasterPage
    {
        get { return (HttpContext.Current.Session[name: "MasterPage"] == null) ? 1 : (int)HttpContext.Current.Session[name: "MasterPage"]; }
        set { HttpContext.Current.Session[name: "MasterPage"] = value; }
    }

    /// <summary>
    /// 表頭的單頁筆數
    /// </summary>
    public static int MasterPageSize
    {
        get { return (HttpContext.Current.Session[name: "MasterPageSize"] == null) ? 1 : (int)HttpContext.Current.Session[name: "MasterPageSize"]; }
        set { HttpContext.Current.Session[name: "MasterPageSize"] = value; }
    }

    /// <summary>
    /// 明細的目前頁數
    /// </summary>
    public static int DetailPage
    {
        get{ return (HttpContext.Current.Session[name: "DetailPage"] == null) ? 1 : (int)HttpContext.Current.Session[name: "DetailPage"]; }
        set{ HttpContext.Current.Session[name: "DetailPage"] = value; }
    }

    /// <summary>
    /// 明細的單頁筆數
    /// </summary>
    public static int DetailPageSize
    {
        get { return (HttpContext.Current.Session[name: "DetailPageSize"] == null) ? 1 : (int)HttpContext.Current.Session[name: "DetailPageSize"]; }
        set { HttpContext.Current.Session[name: "DetailPageSize"] = value; }
    }


    /// <summary>
    /// 第三的單頁筆數
    /// </summary>
    public static int ThirdPage
    {
        get { return (HttpContext.Current.Session[name: "ThirdPage"] == null) ? 1 : (int)HttpContext.Current.Session[name: "ThirdPage"]; }
        set { HttpContext.Current.Session[name: "ThirdPage"] = value; }
    }

    /// <summary>
    /// 第三的單頁筆數
    /// </summary>
    public static int ThirdPageSize
    {
        get { return (HttpContext.Current.Session[name: "ThirdPageSize"] == null) ? 1 : (int)HttpContext.Current.Session[name: "ThirdPageSize"]; }
        set { HttpContext.Current.Session[name: "ThirdPageSize"] = value; }
    }
}
