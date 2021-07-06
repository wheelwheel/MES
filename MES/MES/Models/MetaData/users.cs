using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Windows.Input;

namespace MES.Models
{
    [MetadataType(typeof(usersMetaData))]
    public partial class users
    {
        private class usersMetaData
        {
            [Key]
            public int rowid { get; set; }

            [Display(Name = "帳號")]
            [Required(ErrorMessage = "帳號不可空白!")]
            public string m_account { get; set; }

            [Display(Name = "姓名")]
            [Required(ErrorMessage = "姓名不可空白!")]
            public string m_name { get; set; }

            [Display(Name ="角色")]
            public string role_no { get; set; }

            [Display(Name ="密碼")]
            public string pass_user { get; set; }


            [Display(Name = "信箱")]
            [Required(ErrorMessage = "信箱不可空白!!")]
            [EmailAddress(ErrorMessage = "信箱格式錯誤!")]
            public string email_user { get; set; }

            [Display(Name = "出生日期")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public Nullable<System.DateTime> birthday { get; set; }

            [Display(Name = "住址")]
            [Required(ErrorMessage = "住址不可空白!")]
            public string address { get; set; }

            [Display(Name = "身分證")]
            [Required(ErrorMessage = "身分證不可空白!")]
            [RegularExpression("([A-Z]{1})([1-2]{1})([0-9]{8})", ErrorMessage = "身分證格式錯誤")]
            public string id { get; set; }

            [Display(Name = "電話")]
            [Required(ErrorMessage = "電話不可空白!")]
            [RegularExpression("(09)([0-9]{8})", ErrorMessage = "電話格式錯誤")]
            public string phone { get; set; }



            //public string pic { get; set; }
        }
    }
}