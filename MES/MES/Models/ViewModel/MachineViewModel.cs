using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models.ViewModel
{
    public class MachineViewModel
    {
        [Display(Name = "機台編號")]
        [Required(ErrorMessage = "機台編號不可空白")]
        public string mNo { get; set; }
    }
}
