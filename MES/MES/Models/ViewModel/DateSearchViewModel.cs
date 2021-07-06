using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models
{
    public class DateSearchViewModel
    {
        [Display(Name = "查詢日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime search_date { get; set; }
        public static DateTime search_date_value { get; set; } = DateTime.Today;
        public static DateTime search_date_start
        { get { return DateTime.Parse(search_date_value.ToString("yyyy-MM-dd") + " 00:00:00"); } }

        public static DateTime search_date_end
        { get { return DateTime.Parse(search_date_value.ToString("yyyy-MM-dd") + " 23:59:59"); } }
    }
}