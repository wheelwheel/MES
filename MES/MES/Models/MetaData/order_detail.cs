using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models
{
    [MetadataType(typeof(order_detailMetaData))]
    public partial class order_detail
    {
        private class order_detailMetaData
        {
            [Key]
            public int rowid { get; set; }

            [Display(Name = "訂單編號")]
            public string order_no { get; set; }

            [Display(Name = "產品編號")]
            [Required(ErrorMessage = "編號不可空白!")]
            public string product_no { get; set; }

            [Display(Name = "製令編號")]
            [Required(ErrorMessage = "編號不可空白!")]
            public string workoder_no { get; set; }

            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }
}