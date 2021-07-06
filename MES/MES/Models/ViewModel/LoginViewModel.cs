using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models.ViewModel
{
    public class LoginViewModel
    {
        [Display(Name="登入帳號")]
        [Required(ErrorMessage ="帳號不可為空白")]
        public string Account { get; set; }


        [Display(Name ="登入密碼")]
        [Required(ErrorMessage ="密碼不可空白")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}