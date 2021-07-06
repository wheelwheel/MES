using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models
{
    [MetadataType(typeof(re_complete_detailMetaData))]
    public partial class re_complete_detail
    {
        private class re_complete_detailMetaData
        {
            [Key]
            public int rowid { get; set; }

            [Display(Name = "回報單編號")]
            [Required(ErrorMessage = "編號不可空白!")]
            public string re_complete_no { get; set; }

            [Display(Name = "製程編號")]
            [Required(ErrorMessage = "名稱不可空白!")]
            public string proc_no { get; set; }

            [Display(Name = "機台編號")]
            public string mach_no { get; set; }

            [Display(Name = "數據")]
            public decimal value1 { get; set; }

            [Display(Name = "數據")]
            public decimal value2 { get; set; }

            [Display(Name = "數據")]
            public decimal value3 { get; set; }


            [Display(Name = "員工編號")]
            [Required(ErrorMessage = "編號不可空白!")]
            public string user_no { get; set; }

            [Display(Name = "開始時間")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
            [Required(ErrorMessage = "時間不可空白!")]
            public DateTime start_time { get; set; }

            [Display(Name = "結束時間")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
            [Required(ErrorMessage = "時間不可空白!")]
            public DateTime end_time { get; set; }

            [Display(Name = "數量")]
            [Required(ErrorMessage = "數量不可空白!")]
            public int qty { get; set; }

            [Display(Name = "備註")]
            public string remark { get; set; }

        }
    }
}