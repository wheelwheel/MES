using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models
{
    public class MaterialViewModel
    {
        [Display(Name ="物料編號")]
        [Required(ErrorMessage ="物料編號不可空白")]
        public string mNo { get; set; }


    }


}