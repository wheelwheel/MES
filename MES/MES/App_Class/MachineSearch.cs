using MES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MES.App_Class
{
    public class MachineSearch
    {
        public static string MNO { get; set; } = "";

        public static void Search()
        {
            using (MESEntities db = new MESEntities())
            {
                var data = db.machine.Where(m => m.m_No == MNO).FirstOrDefault();
            }
        }
    }
}