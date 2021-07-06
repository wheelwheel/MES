using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models
{
    [MetadataType(typeof(routeMetaData))]
    public partial class route
    {
        private class routeMetaData
        {
            [Key]
            public int rowid { get; set; }

            [Display(Name = "流程編號")]
            [Required(ErrorMessage = "編號不可空白!")]
            public string r_no { get; set; }

            [Display(Name = "流程名稱")]
            [Required(ErrorMessage = "名稱不可空白!")]
            public string r_name { get; set; }

            [Display(Name = "備註")]
            public string remark { get; set; }

        }
    }
}