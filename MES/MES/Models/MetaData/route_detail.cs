using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models
{
    [MetadataType(typeof(route_detailMetaData))]
    public partial class route_detail
    {
        [Display(Name = "製程名稱")]
        public string proc_name { get; set; }

        private class route_detailMetaData
        {
            [Key]
            public int rowid { get; set; }

            [Display(Name = "流程編號")]
            public string r_no { get; set; }

            [Display(Name = "製程順序")]
            public string sort_no { get; set; }

            [Display(Name = "製程編號")]
            [Required(ErrorMessage = "編號不可空白!")]
            public string proc_no { get; set; }

            [Display(Name = "最大數值")]
            [Required(ErrorMessage = "數值不可空白!")]
            public decimal max_value { get; set; }

            [Display(Name = "最小數值")]
            [Required(ErrorMessage = "數值不可空白!")]
            public decimal min_value { get; set; }

            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }
}