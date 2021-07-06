using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models
{
    [MetadataType(typeof(materialMetaData))]
    public partial class material
    {
        private class materialMetaData
        {  
            [Key]
            public int rowid { get; set; }

            [Display(Name ="物料編號")]
            [Required(ErrorMessage = "編號不可空白!")]
            public string m_No { get; set; }

            [Display(Name = "物料名稱")]
            [Required(ErrorMessage = "名稱不可空白!")]
            public string m_Name { get; set; }

            [Display(Name = "來源工廠")]
            [Required(ErrorMessage = "來源工廠不可空白!")]
            public string factory_no { get; set; }

            [Display(Name = "形狀")]
            public string shape_no { get; set; }

            [Display(Name = "物料邊長")]
            [Required(ErrorMessage = "長度須為數字!")]
            public decimal shape_length { get; set; }

            [Display(Name = "物料直徑")]
            [Required(ErrorMessage = "直徑須為數字!")]
            public decimal shape_diameter { get; set; }

            [Display(Name = "物料長度")]
            [Required(ErrorMessage = "長度不可空白!")]
            public decimal length { get; set; }

            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }
}