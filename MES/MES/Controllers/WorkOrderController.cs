using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES.Models;
using PagedList;

namespace MES.Controllers
{
    public class WorkOrderController : Controller
    {
        MESEntities db = new MESEntities();

        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult List(int page = 0)
        {
            if (page > 0) AppSession.MasterPage = page;
            AppSession.MasterPageSize = 1;
            AppSession.DetailPageSize = 5;
            WorkOrderViewModel model = new WorkOrderViewModel();

            model.workordersList = db.workorder
                .OrderBy(m => m.workorder_no)
                .ToPagedList(AppSession.MasterPage, AppSession.MasterPageSize);

            AppSession.MasterKeyValue = model.workordersList.FirstOrDefault().workorder_no;

            model.workorder_detailsList = db.workorder_detail
                .Where(m => m.workorder_no == AppSession.MasterKeyValue)
               .OrderBy(m => m.sort_no)
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
            var data = db.workorder
                .Where(m => m.workorder_no.Contains(str_today))
                .OrderByDescending(m => m.workorder_no)
                .FirstOrDefault();
            if (data != null)
            {
                if (data.workorder_no.Length == 11)
                    int_seq = int.Parse(data.workorder_no.Substring(8, 3));
            }
            int_seq++;
            string str_workorder_no = str_today + int_seq.ToString().PadLeft(3, '0');

            workorder model = new workorder()
            {
                workorder_no = str_workorder_no,
                workorder_date = DateTime.Now,
                product_no = "",
                route_no = "",
                qty = 0
            };
            ViewBag.ProductList = GetProductList("");
            ViewBag.RouteList = GetRouteList("");
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult CreateMaster(workorder model)
        {
            bool bln_error = false;
            if (!ModelState.IsValid) bln_error = true;
            if (!bln_error)
            {
                var check1 = db.workorder
                    .Where(m => m.workorder_no == model.workorder_no)
                    .FirstOrDefault();
                if (check1 != null) { ModelState.AddModelError("workorder_no", "編號重複"); bln_error = true; }
            }
            if (bln_error)
            {
                ViewBag.ProductList = GetProductList("");
                ViewBag.RouteList = GetRouteList("");
                return View(model);
            }

            workorder data = new workorder()
            {
                workorder_no = AppSession.MasterKeyValue,
                workorder_date = model.workorder_date,
                product_no = model.product_no,
                route_no = model.route_no,
                qty = model.qty
            };

            db.workorder.Add(model);
            db.SaveChanges();

            //新增完計算新資料所在頁數
            AppSession.MasterPage = db.workorder.OrderBy(m => m.workorder_no)
                .ToList().FindIndex(m => m.workorder_no == model.workorder_no);
            AppSession.MasterPage++;
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult CreateDetail()
        {
            var data = db.workorder_detail
                .Where(m => m.workorder_no == AppSession.MasterKeyValue)
                .OrderByDescending(m => m.sort_no)
                .FirstOrDefault();
            string str_sort_no = "01";
            if (data != null)
            {
                if (string.IsNullOrEmpty(data.sort_no)) str_sort_no = "00";
                str_sort_no = (int.Parse(data.sort_no) + 1).ToString().PadLeft(2, '0');
            }

            workorder_detail model = new workorder_detail()
            {
                workorder_no = AppSession.MasterKeyValue,
                sort_no = str_sort_no,
                process_no = "",
                machine_no = "",
                in_qty = 0,
                bad_qty = 0,
                adj_qty = 0,
                out_qty = 0,
                remark = ""
            };
            var in_start = db.workorder.Where(m => m.workorder_no == AppSession.MasterKeyValue).FirstOrDefault();
            if (model.sort_no == "01")
            {
                model.in_qty = (int)in_start.qty;
            }
            ViewBag.ProcessList = GetProcessList("");
            ViewBag.MAchineList = GetMachineList("");
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult CreateDetail(workorder_detail model)
        {
            bool bln_error = false;
            if (!ModelState.IsValid) bln_error = true;
            if (!bln_error)
            {
                var check1 = db.workorder_detail
                    .Where(m => m.workorder_no == model.workorder_no)
                    .Where(m => m.sort_no == model.sort_no)
                    .FirstOrDefault();
                if (check1 != null) { ModelState.AddModelError("sort_no", "編號重複"); bln_error = true; }
            }
            if (bln_error)
            {
                ViewBag.ProcessList = GetProcessList("");
                ViewBag.MAchineList = GetMachineList("");
                return View(model);
            }

            workorder_detail data = new workorder_detail()
            {
                workorder_no = AppSession.MasterKeyValue,
                sort_no = model.sort_no,
                process_no = model.process_no,
                machine_no = model.machine_no,
                in_qty = model.in_qty,
                bad_qty = model.bad_qty,
                adj_qty = model.adj_qty,
                out_qty = Count(model.in_qty, model.bad_qty, model.adj_qty)
            };

            var pre_out_qty = db.workorder_detail.Where(m => m.workorder_no == AppSession.MasterKeyValue).OrderByDescending(m => m.sort_no).FirstOrDefault();

            if (pre_out_qty != null)
            {
                if (data.in_qty != pre_out_qty.out_qty)
                {
                    data.remark = "數量錯誤";
                }
                else
                {
                    data.remark = "";
                }
            }

            db.workorder_detail.Add(data);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        public int Count(int in_qty, int bad_qty, int adj_qty)
        {
            int out_qty = in_qty - adj_qty - bad_qty;
            return out_qty;
        }


        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditMaster(int id)
        {
            var model = db.workorder.Where(m => m.rowid == id).FirstOrDefault();
            ViewBag.RouteList = GetRouteList(model.route_no);
            ViewBag.ProductList = GetProductList(model.product_no);
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditMaster(workorder model)
        {

            bool bln_error = false;

            if (!ModelState.IsValid) bln_error = true;
            if (bln_error)
            {
                ViewBag.RouteList = GetRouteList(model.route_no);
                ViewBag.ProductList = GetProductList(model.product_no);
                return View(model);
            }

            var data = db.workorder.Where(m => m.rowid == model.rowid).FirstOrDefault();
            data.workorder_no = model.workorder_no;
            data.workorder_date = model.workorder_date;
            data.product_no = model.product_no;
            data.route_no = model.route_no;
            data.qty = model.qty;
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditDetail(int id)
        {
            var model = db.workorder_detail.Where(m => m.rowid == id).FirstOrDefault();
            ViewBag.ProcessList = GetProcessList(model.process_no);
            ViewBag.MAchineList = GetMachineList(model.machine_no);
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditDetail(workorder_detail model)
        {
            bool bln_error = false;
            if (!ModelState.IsValid) bln_error = true;
            if (!bln_error)
            {
                var check1 = db.workorder_detail
                    .Where(m => m.rowid != model.rowid)
                    .Where(m => m.workorder_no == model.workorder_no)
                     .Where(m => m.sort_no == model.sort_no)
                    .FirstOrDefault();
                if (check1 != null) { ModelState.AddModelError("sort_no", "編號重複"); bln_error = true; }
            }
            if (bln_error)
            {
                ViewBag.ProcessList = GetProcessList(model.process_no);
                ViewBag.MAchineList = GetMachineList(model.machine_no);
                return View(model);
            }

            var data = db.workorder_detail.Where(m => m.rowid == model.rowid).FirstOrDefault();
            data.sort_no = model.sort_no;
            data.process_no = model.process_no;
            data.machine_no = model.machine_no;
            data.in_qty = model.in_qty;
            data.bad_qty = model.bad_qty;
            data.adj_qty = model.adj_qty;
            data.out_qty = Count(data.in_qty, data.bad_qty, data.adj_qty);

            string pre = (int.Parse(data.sort_no) - 1).ToString().PadLeft(2, '0'); ;

            var pre_out_qty = db.workorder_detail.Where(m => m.workorder_no == AppSession.MasterKeyValue).Where(m => m.sort_no == pre).FirstOrDefault();

            if (pre_out_qty != null)
            {
                if (data.in_qty != pre_out_qty.out_qty)
                {
                    data.remark = "數量錯誤";
                }
                else
                {
                    data.remark = "";
                }
            }

            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult DeleteMaster(int id)
        {
            var model1 = db.workorder.Where(m => m.rowid == id).FirstOrDefault();

            //先刪除明細
            var model2 = db.workorder_detail
                .Where(m => m.workorder_no == model1.workorder_no).ToList();
            foreach (var item in model2)
            {
                db.workorder_detail.Remove(item);
            }
            db.SaveChanges();

            //再刪除表頭
            db.workorder.Remove(model1);
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
            var model = db.workorder_detail.Where(m => m.rowid == id).FirstOrDefault();
            if (model != null)
            {
                db.workorder_detail.Remove(model);
                db.SaveChanges();
            }

            //明細刪除後,頁數 -1
            if (AppSession.DetailPage > 1) AppSession.DetailPage--;

            return RedirectToAction("List");
        }


        /// <summary>
        /// 取得產品下拉資料
        /// </summary>
        /// <param name="productNo">產品代號</param>
        /// <returns></returns>
        private List<SelectListItem> GetProductList(string productNo)
        {
            bool bln_select = false;
            var data = db.product.OrderBy(m => m.product_no).ToList();
            List<SelectListItem> listProduct = new List<SelectListItem>();
            foreach (var item in data)
            {
                SelectListItem newItem = new SelectListItem();
                newItem.Value = item.product_no;
                newItem.Text = item.product_no;
                if (item.product_no == productNo)
                {
                    newItem.Selected = true;
                    bln_select = true;
                }
                else
                    newItem.Selected = false;

                listProduct.Add(newItem);
            }
            if (!bln_select) listProduct.First().Selected = true;
            return listProduct;
        }

        /// <summary>
        /// 取得流程下拉資料
        /// </summary>
        /// <param name="routeNo">流程代號</param>
        /// <returns></returns>
        private List<SelectListItem> GetRouteList(string routeNo)
        {
            bool bln_select = false;
            var data = db.route.OrderBy(m => m.r_no).ToList();
            List<SelectListItem> listRoute = new List<SelectListItem>();
            foreach (var item in data)
            {
                SelectListItem newItem = new SelectListItem();
                newItem.Value = item.r_no;
                newItem.Text = item.r_no;
                if (item.r_no == routeNo)
                {
                    newItem.Selected = true;
                    bln_select = true;
                }
                else
                    newItem.Selected = false;

                listRoute.Add(newItem);
            }
            if (!bln_select) listRoute.First().Selected = true;
            return listRoute;
        }

        /// <summary>
        /// 取得製程下拉資料
        /// </summary>
        /// <param name="processNo">製程代號</param>
        /// <returns></returns>
        private List<SelectListItem> GetProcessList(string processNo)
        {
            bool bln_select = false;
            var data = db.process.OrderBy(m => m.proc_no).ToList();
            List<SelectListItem> listProcess = new List<SelectListItem>();
            foreach (var item in data)
            {
                SelectListItem newItem = new SelectListItem();
                newItem.Value = item.proc_no;
                newItem.Text = item.proc_no;
                if (item.proc_no == processNo)
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
    }
}