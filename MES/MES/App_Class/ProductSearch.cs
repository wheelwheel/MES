using MES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MES.App_Class
{
    public class ProductSearch
    {
        public static string OrderNo { get; set; } = "";

        public static void Search()
        {
            using (MESEntities db = new MESEntities())
            {
                var data = db.re_complete.Where(m => m.order_no == OrderNo).FirstOrDefault();
            }
        }
    }
}