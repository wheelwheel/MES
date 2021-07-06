using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models
{
    [MetadataType(typeof(orderMetaData))]
    public partial class order
    {
        private class orderMetaData
        {
            [Key]
            public int rowid { get; set; }

            [Display(Name = "訂單編號")]
            [Required(ErrorMessage = "編號不可空白!")]
            public string order_no { get; set; }

            [Display(Name = "廠商編號")]
            [Required(ErrorMessage = "名稱不可空白!")]
            public string client_no { get; set; }

            [Display(Name = "訂單日期")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public DateTime order_date { get; set; }

        }
    }
}