using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models
{
    [MetadataType(typeof(workorder_detailMetaData))]
    public partial class workorder_detail
    {
        private class workorder_detailMetaData
        {
            [Key]
            public int rowid { get; set; }

            [Display(Name = "製令編號")]
            [Required(ErrorMessage = "編號不可空白!")]
            public string workorder_no { get; set; }

            [Display(Name = "製程順序")]
            [Required(ErrorMessage = "順序不可空白!")]
            public string sort_no { get; set; }

            [Display(Name = "製程編號")]
            [Required(ErrorMessage = "編號不可空白!")]
            public string process_no { get; set; }

            [Display(Name = "機台編號")]
            [Required(ErrorMessage = "編號不可空白!")]
            public string machine_no { get; set; }

            [Display(Name = "輸入數量")]
            [Required(ErrorMessage = "數量不可空白!")]
            public int in_qty { get; set; }

            [Display(Name = "損壞數量")]
            [Required(ErrorMessage = "數量不可空白!")]
            public int bad_qty { get; set; }

            [Display(Name = "需調整數量")]
            [Required(ErrorMessage = "數量不可空白!")]
            public int adj_qty { get; set; }

            [Display(Name = "輸出數量")]
            [Required(ErrorMessage = "數量不可空白!")]
            public int out_qty { get; set; }

            [Display(Name = "備註")]
            public string remark { get; set; }

        }
    }
}