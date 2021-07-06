using MES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public static class MaterialSearch
{
    public static string MNO { get; set; } = "";

    public static void Search()
    {
        using (MESEntities db = new MESEntities())
        {
            var data = db.material.Where(m => m.m_No == MNO).FirstOrDefault();
        }
    }
}
