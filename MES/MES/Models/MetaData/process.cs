using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models
{
    [MetadataType(typeof(processMetaData))]
    public partial class process
    {
        private class processMetaData
        {
            [Key]
            public int rowid { get; set; }

            [Display(Name = "製程編號")]
            [Required(ErrorMessage = "編號不可空白!")]
            public string proc_no { get; set; }

            [Display(Name = "製程名稱")]
            [Required(ErrorMessage = "名稱不可空白!")]
            public string proc_name { get; set; }

            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }
}