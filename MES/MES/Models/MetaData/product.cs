using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models
{
    [MetadataType(typeof(productMetaData))]
    public partial class product
    {
       private class productMetaData
        {
            [Key]
            public int rowid { get; set; }

            //[Display(Name ="訂單編號")]
            //public string order_no { get; set; }

            [Display(Name = "產品編號")]
            public string product_no { get; set; }

            [Display(Name = "產品名稱")]
            public string product_name { get; set; }

            [Display(Name = "是否為成品")]
            public string role { get; set; }

            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }
}