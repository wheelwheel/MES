using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace MES.Models
{
    public class ResetPasswordViewModel
    {

        [Key]
        [Display(Name = "登入帳號")]
        public string UserNo { get; set; }

        [Required(ErrorMessage = "請輸入目前的密碼")]
        [DataType(DataType.Password)]
        [Display(Name = "目前密碼")]
        public string CurrentPassword { get; set; }


        [Display(Name = "新的密碼")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "新的密碼不可空白!!")]
        public string NewPassword { get; set; }


        [Display(Name = "確認密碼")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "確認密碼不可空白!!")]
        [Compare(otherProperty: "NewPassword", ErrorMessage = "確認密碼與新密碼不符")]
        public string ConfirmPassword { get; set; }
    }
}