using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models
{
    public class ProductViewModel
    {
        [Display(Name = "完工單編號")]
        public string re_complete_no { get; set; }

        [Display(Name = "訂單編號")]
        public string order_no { get; set; }

        [Display(Name = "產品編號")]
        public string product_no { get; set; }

        [Display(Name = "產品名稱")]
        public string product_name { get; set; }

        [Display(Name = "製程編號")]
        public string proc_no { get; set; }

        [Display(Name = "製程名稱")]
        public string proc_name { get; set; }

        [Display(Name = "機台編號")]
        public string mach_no { get; set; }

        [Display(Name = "機台名稱")]
        public string mach_name { get; set; }

        [Display(Name = "半徑(mm)")]
        public string Diameter { get; set; }

        [Display(Name = "長度")]
        public string Length { get; set; }

        [Display(Name = "誤差")]
        public string Diameter_Tolerance { get; set; }

        [Display(Name = "真直度")]
        public string Straightness { get; set; }

        [Display(Name = "操作員編號")]
        public string user_no { get; set; }

        [Display(Name = "開始時間")]
        public Nullable<System.DateTime> start_time { get; set; }

        [Display(Name = "結束時間")]
        public Nullable<System.DateTime> end_time { get; set; }

        [Display(Name = "數量")]
        public Nullable<int> qty { get; set; }



    }
}