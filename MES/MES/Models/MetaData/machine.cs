using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models
{
    [MetadataType(typeof(machineMetaData))]
    public partial class machine
    {
        private class machineMetaData
        {
            [Key]
            public int rowid { get; set; }

            [Display(Name ="機台編號")]
            public string m_No { get; set; }

            [Display(Name = "機台名稱")]
            public string m_Name { get; set; }

            [Display(Name = "運作情況")]
            public string status { get; set; }

            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }
}