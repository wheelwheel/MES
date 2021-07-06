using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models.ViewModel
{
    public class MachieDataViewModel
    {
        [Display(Name ="機台編號")]
        public string m_No { get; set; }

        [Display(Name = "機台名稱")]
        public string m_Name { get; set; }

        [Display(Name = "機台數據1")]
        public Nullable<System.Decimal> value1 { get; set; }

        [Display(Name = "機台數據2")]
        public Nullable<System.Decimal> value2 { get; set; }

        [Display(Name = "機台數據3")]
        public Nullable<System.Decimal> value3 { get; set; }

        [Display(Name = "開始時間")]
        public Nullable<System.DateTime> start_time { get; set; }

        [Display(Name = "結束時間")]
        public Nullable<System.DateTime> end_time { get; set; }

        [Display(Name = "成功率")]
        public decimal success { get; set; }

        [Display(Name = "小時")]
        public int hour { get; set; }

    }
}