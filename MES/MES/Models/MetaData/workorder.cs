using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models
{
    [MetadataType(typeof(workorderMetaData))]
    public partial class workorder
    {
        private class workorderMetaData
        {
            [Key]
            public int rowid { get; set; }

            [Display(Name = "製令編號")]
            [Required(ErrorMessage = "編號不可空白!")]
            public string workorder_no { get; set; }

            [Display(Name = "製令日期")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            [Required(ErrorMessage = "日期不可空白!")]
            public Nullable<System.DateTime> workorder_date { get; set; }

            //[Display(Name = "訂單編號")]
            //[Required(ErrorMessage = "編號不可空白!")]
            //public string order_no { get; set; }

            [Display(Name = "產品編號")]
            [Required(ErrorMessage = "編號不可空白!")]
            public string product_no { get; set; }

            [Display(Name = "流程編號")]
            [Required(ErrorMessage = "編號不可空白!")]
            public string route_no { get; set; }

            [Display(Name = "數量")]
            [Required(ErrorMessage = "數量不可空白!")]
            public Nullable<int> qty { get; set; }

            [Display(Name = "備註")]
            public string remark { get; set; }

        }
    }
}