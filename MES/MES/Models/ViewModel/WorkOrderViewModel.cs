using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models
{
    public class WorkOrderViewModel
    {

        public IPagedList<order_detail> order_detailsList { get; set; }
        public IPagedList<workorder> workordersList { get; set; }
        public IPagedList<workorder_detail> workorder_detailsList { get; set; }


        //[Display(Name="製令編號")]
        //public string workorder_no { get; set; }

        //[Display(Name = "製令日期")]
        //public DateTime workorder_date { get; set; }

        //[Display(Name = "訂單編號")]
        //public string order_no { get; set; }

        //[Display(Name = "產品編號")]
        //public string product_no { get; set; }

        //[Display(Name = "流程編號")]
        //public string route_no { get; set; }

        //[Display(Name = "數量")]
        //public int qty { get; set; }

        //[Display(Name = "製程順序")]
        //public string sort_no { get; set; }

        //[Display(Name = "製程編號")]
        //public string process_no { get; set; }

        //[Display(Name = "機台編號")]
        //public string machine_no { get; set; }

        //[Display(Name = "輸入數量")]
        //public int in_qty { get; set; }

        //[Display(Name = "損壞數量")]
        //public int bad_qty { get; set; }

        //[Display(Name = "需調整數量")]
        //public int adj_qty { get; set; }

        //[Display(Name = "輸出數量")]
        //public int out_qty { get; set; }
    }
}