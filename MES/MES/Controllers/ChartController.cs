using MES.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MES.Controllers
{
    public class ChartController : Controller
    {
        MESEntities db = new MESEntities();


        // GET: Chart
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult Multi_Axis_Line_Chart(string id = "A01")
        {
            int int_hour = 1;
            int int_hour1 = 1;
            int int_minute = 2;
            int int_hours_start = 0;
            int int_hours_end = 0;
            int int_index = 0;
            string str_hour = "";
            string str_minute = "";
            DateTime dateStartTime;
            DateTime dateEndTime;
            decimal dataValue1 = 0;

            DateTime now = DateTime.Now;

            List<string> labelList = new List<string>();
            List<decimal?> dataList = new List<decimal?>();

            List<TimeSpan> labelList2 = new List<TimeSpan>();
            List<decimal> dataList2 = new List<decimal>();
            List<int> dataList3 = new List<int>();
            List<int> dataList4 = new List<int>();


            for (int i = 8; i < 20; i += int_hour)
            {
                str_hour = i.ToString().PadLeft(2, '0');
                for (int j = 0; j < 60; j += int_minute)
                {
                    str_minute = j.ToString().PadLeft(2, '0');
                    labelList.Add(str_hour + str_minute);
                    dataList.Add(0);
                }

            }


            var valuelist = db.re_complete_detail
                .Join(db.re_complete, p => p.re_complete_no, d => d.re_complete_no,
                (p1, d1) => new { p1, d1 })
                .Join(db.order_detail, p => p.d1.order_no, d => d.order_no,
                (p2, d2) => new { p2, d2 })
                .Join(db.workorder_detail, p => p.d2.workoder_no, d => d.workorder_no,
                (p3, d3) => new { p3, d3, success = Math.Round(((decimal)((decimal)d3.out_qty / d3.in_qty) * 100), 2) })
                .Where(m => m.d3.machine_no == m.p3.p2.p1.mach_no);

            var valueList = valuelist
                .Where(m => m.p3.p2.p1.start_time >= DateSearchViewModel.search_date_start)
                .Where(m => m.p3.p2.p1.start_time <= DateSearchViewModel.search_date_end)
                .Where(m => m.p3.p2.p1.mach_no == id)
                .OrderBy(m => m.p3.p2.p1.rowid)
                .ToList();

            foreach (var item in valueList)
            {
                dateStartTime = item.p3.p2.p1.start_time;
                string DateStartTime = dateStartTime.ToString("HH:mm").Replace(":", "");

                dateEndTime = item.p3.p2.p1.end_time;
                string DateEndTime = dateEndTime.ToString("HH:mm").Replace(":", "");

                dataValue1 = item.p3.p2.p1.value1.GetValueOrDefault();
                int_hours_start = int.Parse(DateStartTime.ToString());
                int_hours_end = int.Parse(DateEndTime.ToString());
                int_index = 0;
                foreach (var h in labelList)
                {
                    int_hour1 = int.Parse(h.Substring(0, 4));
                    if (int_hours_start <= int_hour1 && int_hours_end >= int_hour1)
                    {
                        dataList[int_index] += dataValue1;

                    }
                    int_index++;
                }
            }

            for (int i = 0; i < labelList.Count; i++)
            {
                if (labelList[i].Substring(2, 2) != "00")
                {
                    labelList[i] = "";
                }
            }

            labelList2 = valueList.Select(selector: m => m.p3.p2.p1.end_time.TimeOfDay).ToList();
            dataList2 = valueList.Select(m => m.success).ToList();

            dataList3 = valueList.Select(m => m.d3.adj_qty).ToList();
            dataList4 = valueList.Select(m => m.d3.bad_qty).ToList();

            ViewBag.LabelList = JsonConvert.SerializeObject(labelList);
            ViewBag.DataList = JsonConvert.SerializeObject(dataList);


            ViewBag.LabelList2 = JsonConvert.SerializeObject(labelList2);
            ViewBag.DataList2 = JsonConvert.SerializeObject(dataList2);
            ViewBag.DataList3 = JsonConvert.SerializeObject(dataList3);
            ViewBag.DataList4 = JsonConvert.SerializeObject(dataList4);

            ViewBag.MachineName = id.ToString();
            

            DateSearchViewModel model = new DateSearchViewModel();
            model.search_date = DateSearchViewModel.search_date_value;
            ViewBag.Date =  model.search_date.ToString("d");
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult Search(DateSearchViewModel model)
        {
            DateSearchViewModel.search_date_value = model.search_date;
            return RedirectToAction("Multi_Axis_Line_Chart", "Chart");
        }
    }
}