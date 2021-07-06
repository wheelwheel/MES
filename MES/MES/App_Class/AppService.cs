using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// 應用程式服務類別
/// </summary>
public static class AppService
{
    /// <summary>
    /// 應用程式名稱
    /// </summary>
    public static string AppName
    {
        get
        {
            object obj_value = WebConfigurationManager.AppSettings["AppName"];
            return (obj_value == null) ? "未定義名稱" : obj_value.ToString(); 
        }
    }
    /// <summary>
    /// 除錯模式
    /// </summary>
    public static bool DebugMode
    {
        get
        {
            object obj_value = WebConfigurationManager.AppSettings["DebugMode"];
            string str_value = (obj_value == null) ? "0" : obj_value.ToString();
            return (str_value == "1");
        }
    }
}
