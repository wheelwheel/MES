using MES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;


namespace MES.Controllers
{
    public class RouteController : Controller
    {
        MESEntities db = new MESEntities();

        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult List( int page=0)
        {
            if (page > 0) AppSession.MasterPage = page;
            AppSession.MasterPageSize = 1;
            AppSession.DetailPageSize =5;
            RouteViewModel model = new RouteViewModel();
            model.routesList = db.route.OrderBy(m => m.r_no)
                .ToPagedList(AppSession.MasterPage, AppSession.MasterPageSize);

            //保存目前表頭的主鍵值
            AppSession.MasterKeyValue = model.routesList.FirstOrDefault().r_no;

            model.route_detailsList = db.route_detail
                .Where(m => m.r_no == AppSession.MasterKeyValue)
                .OrderBy(m => m.sort_no)
                .ToPagedList(AppSession.DetailPage, AppSession.DetailPageSize);

            foreach (var item in model.route_detailsList)
            {
                item.proc_name = GetProcessName(item.proc_no);
            }

            return View(model);
        }


        public ActionResult MasterPage(int page = 1)
        {
            AppSession.MasterPage = page;
            AppSession.DetailPage = 1;
            return RedirectToAction("List");
        }

        public ActionResult DetailPage(int page = 1)
        {
            AppSession.DetailPage = page;
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult CreateMaster()
        {
            var data=db.route.OrderByDescending(m => m.r_no)
                .FirstOrDefault();

            string str_route_no = "001";
            if (data != null)
            {
                if (string.IsNullOrEmpty(data.r_no)) str_route_no = "001";
                str_route_no = (int.Parse(data.r_no) + 1).ToString().PadLeft(3, '0');
            }

            route model = new route()
            {
                r_no = str_route_no,
                r_name = ""
            };
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult CreateMaster(route model)
        {
            if (!ModelState.IsValid) return View(model);

            bool bln_error = false;
            var check1 = db.route.Where(m => m.r_no == model.r_no).FirstOrDefault();
            if (check1 != null) { ModelState.AddModelError("r_no", "編號重複"); bln_error = true; }
            var check2 = db.route.Where(m => m.r_name == model.r_name).FirstOrDefault();
            if (check2 != null) { ModelState.AddModelError("r_name", "名稱重複!"); bln_error = true; }
            if (bln_error) return View(model);

            db.route.Add(model);
            db.SaveChanges();

            //新增完計算新資料所在頁數
            AppSession.MasterPage = db.route.OrderBy(m => m.r_no)
                .ToList().FindIndex(m => m.r_no == model.r_no);
            AppSession.MasterPage++;
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult CreateDetail()
        {
            var data = db.route_detail
                .Where(m => m.r_no == AppSession.MasterKeyValue)
                .OrderByDescending(m => m.sort_no)
                .FirstOrDefault();
            string str_sort_no = "01";
            if (data != null)
            {
                if (string.IsNullOrEmpty(data.sort_no)) str_sort_no = "00";
                str_sort_no = (int.Parse(data.sort_no) + 1).ToString().PadLeft(2, '0');
            }

            route_detail model = new route_detail()
            {
                r_no = AppSession.MasterKeyValue,
                sort_no = str_sort_no,
                proc_no = "",
                max_value = 0,
                min_value = 0,
                remark = ""
            };
            ViewBag.ProcessList = GetProcessList("");
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult CreateDetail(route_detail model)
        {
            bool bln_error = false;
            if (!ModelState.IsValid) bln_error = true;
            if (!bln_error)
            {
                var check1 = db.route_detail
                    .Where(m => m.r_no == model.r_no)
                    .Where(m => m.proc_no == model.proc_no)
                    .FirstOrDefault();
                if (check1 != null) { ModelState.AddModelError("proc_no", "編號重複"); bln_error = true; }
            }
            if (bln_error)
            {
                ViewBag.ProcessList = GetProcessList("");
                return View(model);
            }

            route_detail data = new route_detail()
            {
                r_no = AppSession.MasterKeyValue,
                sort_no = model.sort_no,
                proc_no = model.proc_no,
                max_value = model.max_value,
                min_value = model.min_value,
                remark = model.remark
            };

            db.route_detail.Add(data);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditMaster(int id)
        {
            var model = db.route.Where(m => m.rowid == id).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditMaster(route model)
        {
            if (!ModelState.IsValid) return View(model);

            bool bln_error = false;
            var check1 = db.route.Where(m => m.r_no == model.r_no && m.rowid != model.rowid).FirstOrDefault();
            if (check1 != null) { ModelState.AddModelError("r_no", "編號重複"); bln_error = true; }
            var check2 = db.route.Where(m => m.r_name == model.r_name && m.rowid != model.rowid).FirstOrDefault();
            if (check2 != null) { ModelState.AddModelError("r_name", "名稱重複!"); bln_error = true; }
            if (bln_error) return View(model);

            var data = db.route.Where(m => m.rowid == model.rowid).FirstOrDefault();
            data.r_no = model.r_no;
            data.r_name = model.r_name;
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditDetail(int id)
        {
            var model = db.route_detail.Where(m => m.rowid == id).FirstOrDefault();
            ViewBag.ProcessList = GetProcessList(model.proc_no);
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditDetail(route_detail model)
        {
            bool bln_error = false;
            if (!ModelState.IsValid) bln_error = true;
            if (!bln_error)
            {
                var check1 = db.route_detail
                    .Where(m => m.rowid != model.rowid)
                    .Where(m => m.r_no == model.r_no)
                     .Where(m => m.proc_no == model.proc_no)
                    .FirstOrDefault();
                if (check1 != null) { ModelState.AddModelError("proc_no", "編號重複"); bln_error = true; }
            }
            if (bln_error)
            {
                ViewBag.ProcessList = GetProcessList(model.proc_no);
                return View(model);
            }

            var data = db.route_detail.Where(m => m.rowid == model.rowid).FirstOrDefault();
            data.sort_no = model.sort_no;
            data.proc_no = model.proc_no;
            data.min_value = model.min_value;
            data.max_value = model.max_value;
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult DeleteMaster(int id)
        {
            var model1 = db.route.Where(m => m.rowid == id).FirstOrDefault();

            //先刪除明細
            var model2 = db.route_detail
                .Where(m => m.r_no == model1.r_no).ToList();
            foreach (var item in model2)
            {
                db.route_detail.Remove(item);
            }
            db.SaveChanges();

            //再刪除表頭
            db.route.Remove(model1);
            db.SaveChanges();

            //表頭刪除後,表頭頁數 -1 , 明細從第 1 頁開始
            if (AppSession.MasterPage > 1) AppSession.MasterPage--;
            AppSession.DetailPage = 1;
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult DeleteDetail(int id)
        {
            var model = db.route_detail.Where(m => m.rowid == id).FirstOrDefault();
            if (model != null)
            {
                db.route_detail.Remove(model);
                db.SaveChanges();
            }

            //明細刪除後,頁數 -1
            if (AppSession.DetailPage > 1) AppSession.DetailPage--;

            return RedirectToAction("List");
        }

        /// <summary>
        /// 取得製程名稱
        /// </summary>
        /// <param name="procNo">製程代號</param>
        /// <returns></returns>
        private string GetProcessName(string procNo)
        {
            var data = db.process.Where(m => m.proc_no == procNo).FirstOrDefault();
            return (data == null) ? "" : data.proc_name;
        }

        /// <summary>
        /// 取得製程下拉資料
        /// </summary>
        /// <param name="procNo">製程代號</param>
        /// <returns></returns>
        private List<SelectListItem> GetProcessList(string procNo)
        {
            bool bln_select = false;
            var data = db.process.OrderBy(m => m.proc_no).ToList();
            List<SelectListItem> listProcess = new List<SelectListItem>();
            foreach (var item in data)
            {
                SelectListItem newItem = new SelectListItem();
                newItem.Value = item.proc_no;
                newItem.Text = item.proc_name;
                if (item.proc_no == procNo)
                {
                    newItem.Selected = true;
                    bln_select = true;
                }
                else
                    newItem.Selected = false;

                listProcess.Add(newItem);
            }
            if (!bln_select) listProcess.First().Selected = true;
            return listProcess;
        }
    }
}