using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES.Models;
using MES.Models.ViewModel;
using PagedList;

namespace MES.Controllers
{
    public class Re_CompleteController : Controller
    {
        MESEntities db = new MESEntities();

        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult List(int page=0)
        {
            if (page > 0) AppSession.MasterPage = page;
            AppSession.MasterPageSize = 1;
            AppSession.DetailPageSize = 5;
            re_completeViewModel model = new re_completeViewModel();
            model.re_completesList = db.re_complete.OrderBy(m => m.re_complete_no)
                .ToPagedList(AppSession.MasterPage, AppSession.MasterPageSize);

            //保存目前表頭的主鍵值
            AppSession.MasterKeyValue = model.re_completesList.FirstOrDefault().re_complete_no;

            model.re_complete_detailsList = db.re_complete_detail
                .Where(m => m.re_complete_no == AppSession.MasterKeyValue)
                .OrderBy(m => m.re_complete_no)
                .ToPagedList(AppSession.DetailPage, AppSession.DetailPageSize);

            return View(model);
        }

        public ActionResult MasterPage(int page = 1)
        {
            AppSession.MasterPage = page;
            AppSession.DetailPageSize = 1;
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
            int int_seq = 0;
            string str_today = DateTime.Now.ToString("yyyyMMdd");
            var data = db.re_complete
                .Where(m => m.re_complete_no.Contains(str_today))
                .OrderByDescending(m => m.re_complete_no)
                .FirstOrDefault();
            if (data != null)
            {
                if (data.re_complete_no.Length == 11)
                    int_seq = int.Parse(data.re_complete_no.Substring(8, 3));
            }
            int_seq++;
            string str_re_complete_no = str_today + int_seq.ToString().PadLeft(3, '0');

            re_complete model = new re_complete()
            {
                re_complete_no = str_re_complete_no,
                order_no="",
                re_complete_date = DateTime.Now
            };
            ViewBag.OrderList = GetOrderList("");
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult CreateMaster(re_complete model)
        {
            bool bln_error = false;
            //if (!ModelState.IsValid) return View(model);


            //var check1 = db.re_complete.Where(m => m.re_complete_no == model.re_complete_no).FirstOrDefault();
            //if (check1 != null) { ModelState.AddModelError("re_complete_no", "編號重複"); bln_error = true; }

            //if (bln_error) return View(model);

            if (!ModelState.IsValid) bln_error = true;
            if (!bln_error)
            {
                var check1 = db.re_complete
                    .Where(m => m.re_complete_no == model.re_complete_no)
                    .FirstOrDefault();
                if (check1 != null) { ModelState.AddModelError("re_complete_no", "編號重複"); bln_error = true; }
            }
            if (bln_error)
            {

                ViewBag.OrderList = GetOrderList("");
                return View(model);
            }

            re_complete data = new re_complete()
            {
                re_complete_no = AppSession.MasterKeyValue,
                order_no=model.order_no,
                re_complete_date = model.re_complete_date
            };

            db.re_complete.Add(model);
            db.SaveChanges();

            //新增完計算新資料所在頁數
            AppSession.MasterPage = db.re_complete.OrderBy(m => m.re_complete_no)
                .ToList().FindIndex(m => m.re_complete_no == model.re_complete_no);
            AppSession.MasterPage++;
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult CreateDetail()
        {
            var data = db.re_complete_detail
                .Where(m => m.re_complete_no == AppSession.MasterKeyValue)
                .OrderByDescending(m => m.proc_no)
                .FirstOrDefault();


            re_complete_detail model = new re_complete_detail()
            {
                re_complete_no = AppSession.MasterKeyValue,
                proc_no = "",
                mach_no = "",
                value1 = 0,
                value2 = 0,
                value3 = 0,
                user_no = "",
                start_time = DateTime.Now,
                end_time = DateTime.Now,
                qty = 0,
                remark = ""
            };
            ViewBag.ProcessList = GetProcessList("");
            ViewBag.MachineList = GetMachineList("");
            ViewBag.UserList = GetUserList("");
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult CreateDetail(re_complete_detail model)
        {
            bool bln_error = false;
            if (!ModelState.IsValid) bln_error = true;
            if (!bln_error)
            {
                var check1 = db.re_complete_detail
                    .Where(m => m.re_complete_no == model.re_complete_no)
                    //.Where(m => m.proc_no == model.proc_no)
                    .FirstOrDefault();
                //if (check1 != null) { ModelState.AddModelError("proc_no", "編號重複"); bln_error = true; }
            }
            if (bln_error)
            {
                ViewBag.ProcessList = GetProcessList("");
                ViewBag.MachineList = GetMachineList("");
                ViewBag.UserList = GetUserList("");
                return View(model);
            }

            re_complete_detail data = new re_complete_detail()
            {

                re_complete_no = AppSession.MasterKeyValue,
                proc_no = model.proc_no,
                mach_no = model.mach_no,
                value1 = model.value1,
                value2 = model.value2,
                value3 = model.value3,
                user_no = model.user_no,
                start_time = model.start_time,
                end_time = model.end_time,
                qty = model.qty,
                remark = model.remark
            };

            db.re_complete_detail.Add(data);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditMaster(int id)
        {
            var model = db.re_complete.Where(m => m.rowid == id).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditMaster(re_complete model)
        {
            if (!ModelState.IsValid) return View(model);

            bool bln_error = false;
            var check1 = db.re_complete.Where(m => m.re_complete_no == model.re_complete_no && m.rowid != model.rowid).FirstOrDefault();
            if (check1 != null) { ModelState.AddModelError("re_complete_no", "編號重複"); bln_error = true; }
            if (bln_error) return View(model);

            var data = db.re_complete.Where(m => m.rowid == model.rowid).FirstOrDefault();
            data.re_complete_no = model.re_complete_no;
            data.re_complete_date = model.re_complete_date;
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditDetail(int id)
        {
            var model = db.re_complete_detail.Where(m => m.rowid == id).FirstOrDefault();
            ViewBag.ProcessList = GetProcessList(model.proc_no);
            ViewBag.MachineList = GetMachineList(model.mach_no);
            ViewBag.UserList = GetUserList(model.user_no);
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditDetail(re_complete_detail model)
        {
            bool bln_error = false;
            if (!ModelState.IsValid) bln_error = true;
            if (!bln_error)
            {
                var check1 = db.re_complete_detail
                    .Where(m => m.rowid != model.rowid)
                    .Where(m => m.re_complete_no == model.re_complete_no)
                    //.Where(m => m.proc_no == model.proc_no)
                    .FirstOrDefault();
                //if (check1 != null) { ModelState.AddModelError("proc_no", "編號重複"); bln_error = true; }
            }
            if (bln_error)
            {
                ViewBag.ProcessList = GetProcessList(model.proc_no);
                ViewBag.MachineList = GetMachineList(model.mach_no);
                ViewBag.UserList = GetUserList(model.user_no);
                return View(model);
            }

            var data = db.re_complete_detail.Where(m => m.rowid == model.rowid).FirstOrDefault();
            data.proc_no = model.proc_no;
            data.mach_no = model.mach_no;
            data.value1 = model.value1;
            data.value2 = model.value2;
            data.value3 = model.value3;
            data.user_no = model.user_no;
            data.start_time = model.start_time;
            data.end_time = model.end_time;
            data.qty = model.qty;
            data.remark = model.remark;
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult DeleteMaster(int id)
        {
            var model1 = db.re_complete.Where(m => m.rowid == id).FirstOrDefault();

            //先刪除明細
            var model2 = db.re_complete_detail
                .Where(m => m.re_complete_no == model1.re_complete_no).ToList();
            foreach (var item in model2)
            {
                db.re_complete_detail.Remove(item);
            }
            db.SaveChanges();

            //再刪除表頭
            db.re_complete.Remove(model1);
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
            var model = db.re_complete_detail.Where(m => m.rowid == id).FirstOrDefault();
            if (model != null)
            {
                db.re_complete_detail.Remove(model);
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
        /// 取得訂單下拉資料
        /// </summary>
        /// <param name="orderNo">訂單代號</param>
        /// <returns></returns>
        private List<SelectListItem> GetOrderList(string orderNo)
        {
            bool bln_select = false;
            var data = db.order.OrderBy(m => m.order_no).ToList();
            List<SelectListItem> listOrders = new List<SelectListItem>();
            foreach (var item in data)
            {
                SelectListItem newItem = new SelectListItem();
                newItem.Value = item.order_no;
                newItem.Text = item.order_no;
                if (item.order_no == orderNo)
                {
                    newItem.Selected = true;
                    bln_select = true;
                }
                else
                    newItem.Selected = false;

                listOrders.Add(newItem);
            }
            if (!bln_select) listOrders.First().Selected = true;
            return listOrders;
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
                newItem.Text = item.proc_no;
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

        /// <summary>
        /// 取得機台下拉資料
        /// </summary>
        /// <param name="machineNo">機台代號</param>
        /// <returns></returns>
        private List<SelectListItem> GetMachineList(string machineNo)
        {
            bool bln_select = false;
            var data = db.machine.OrderBy(m => m.m_No).ToList();
            List<SelectListItem> listMachine = new List<SelectListItem>();
            foreach (var item in data)
            {
                SelectListItem newItem = new SelectListItem();
                newItem.Value = item.m_No;
                newItem.Text = item.m_No;
                if (item.m_No == machineNo)
                {
                    newItem.Selected = true;
                    bln_select = true;
                }
                else
                    newItem.Selected = false;

                listMachine.Add(newItem);
            }
            if (!bln_select) listMachine.First().Selected = true;
            return listMachine;
        }

        /// <summary>
        /// 取得員工下拉資料
        /// </summary>
        /// <param name="userNo">員工代號</param>
        /// <returns></returns>
        private List<SelectListItem> GetUserList(string userNo)
        {
            bool bln_select = false;
            var data = db.users.Where(m=>m.role_no=="U").OrderBy(m => m.m_account).ToList();
            List<SelectListItem> listUser = new List<SelectListItem>();
            foreach (var item in data)
            {
                SelectListItem newItem = new SelectListItem();
                newItem.Value = item.m_account;
                newItem.Text = item.m_account;
                if (item.m_account == userNo)
                {
                    newItem.Selected = true;
                    bln_select = true;
                }
                else
                    newItem.Selected = false;

                listUser.Add(newItem);
            }
            if (!bln_select) listUser.First().Selected = true;
            return listUser;
        }
    }
}