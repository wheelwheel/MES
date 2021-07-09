using MES.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MES.Controllers
{
    public class OrderController : Controller
    {
        MESEntities db = new MESEntities();

        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult List(int page=0)
        {
            if (page > 0) AppSession.MasterPage = page;
            AppSession.MasterPageSize = 1;
            AppSession.DetailPageSize = 5;
            OrderViewModel model = new OrderViewModel();
            model.ordersList = db.order.OrderBy(m => m.order_no)
                .ToPagedList(AppSession.MasterPage, AppSession.MasterPageSize);

            //保存目前表頭的主鍵值
            AppSession.MasterKeyValue = model.ordersList.FirstOrDefault().order_no;

            model.order_detailsList = db.order_detail
                .Where(m => m.order_no == AppSession.MasterKeyValue)
                .OrderBy(m => m.product_no)
                .OrderBy(m => m.workoder_no)
                .ToPagedList(AppSession.DetailPage, AppSession.DetailPageSize);

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
            int int_seq = 0;
            string str_today = DateTime.Now.ToString("yyyyMMdd");
            var data = db.order
                .Where(m => m.order_no.Contains(str_today))
                .OrderByDescending(m => m.order_no)
                .FirstOrDefault();
            if (data != null)
            {
                if (data.order_no.Length == 11)
                    int_seq = int.Parse(data.order_no.Substring(8, 3));
            }
            int_seq++;
            string str_order_no = str_today + int_seq.ToString().PadLeft(3, '0');

            order model = new order()
            {
                order_no = str_order_no,
                client_no = "",
                order_date=DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult CreateMaster(order model)
        {
            if (!ModelState.IsValid) return View(model);

            bool bln_error = false;
            var check1 = db.order.Where(m => m.order_no == model.order_no).FirstOrDefault();
            if (check1 != null) { ModelState.AddModelError("order_no", "編號重複"); bln_error = true; }
            if (bln_error) return View(model);

            db.order.Add(model);
            db.SaveChanges();

            //新增完計算新資料所在頁數
            AppSession.MasterPage = db.order.OrderBy(m => m.order_no)
                .ToList().FindIndex(m => m.order_no == model.order_no);
            AppSession.MasterPage++;
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult CreateDetail()
        {
            int int_seq = 0;
            string str_today = DateTime.Now.ToString("yyyyMMdd");
            var data = db.order_detail
                .Where(m => m.order_no.Contains(str_today))
                .OrderByDescending(m => m.order_no)
                .FirstOrDefault();
            if (data != null)
            {
                if (data.order_no.Length == 11)
                    int_seq = int.Parse(data.order_no.Substring(8, 3));
            }
            int_seq++;
            string str_workoder_no = str_today + int_seq.ToString().PadLeft(3, '0');


            order_detail model = new order_detail()
            {
                order_no = AppSession.MasterKeyValue,
                product_no = "",
                workoder_no = str_workoder_no,
                remark = ""
            };

            ViewBag.ProductList = GetProductList("");
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult CreateDetail(order_detail model)
        {
            bool bln_error = false;
            if (!ModelState.IsValid) bln_error = true;

            var check1 = db.order_detail.Where(m => m.workoder_no == model.workoder_no).FirstOrDefault();
            if (check1 != null) { ModelState.AddModelError("workoder_no", "編號重複"); bln_error = true; }

            if (!bln_error)
            {
                 check1 = db.order_detail
                    .Where(m => m.order_no == model.order_no)
                    .Where(m => m.product_no == model.product_no)
                    .FirstOrDefault();
            }
            if (bln_error)
            {
                ViewBag.ProductList = GetProductList("");
                return View(model);
            }

            order_detail data = new order_detail()
            {
                order_no = AppSession.MasterKeyValue,
                product_no = model.product_no,
                workoder_no = model.workoder_no,
                remark = model.remark
            };

            db.order_detail.Add(data);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditMaster(int id)
        {
            var model = db.order.Where(m => m.rowid == id).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditMaster(order model)
        {
            if (!ModelState.IsValid) return View(model);

            bool bln_error = false;
            var check1 = db.order.Where(m => m.order_no == model.order_no && m.rowid != model.rowid).FirstOrDefault();
            if (check1 != null) { ModelState.AddModelError("order_no", "編號重複"); bln_error = true; }
            if (bln_error) return View(model);

            var data = db.order.Where(m => m.rowid == model.rowid).FirstOrDefault();
            data.order_no = model.order_no;
            data.client_no = model.client_no;
            data.order_date = model.order_date;
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditDetail(int id)
        {
            var model = db.order_detail.Where(m => m.rowid == id).FirstOrDefault();
            ViewBag.ProductList = GetProductList(model.product_no);
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditDetail(order_detail model)
        {
            bool bln_error = false;
            if (!ModelState.IsValid) bln_error = true;
            if (!bln_error)
            {
                var check1 = db.order_detail
                    .Where(m => m.rowid != model.rowid)
                    .Where(m => m.order_no == model.order_no)
                     .Where(m => m.product_no == model.product_no)
                    .FirstOrDefault();
            }
            if (bln_error)
            {
                ViewBag.ProductList = GetProductList(model.product_no);
                return View(model);
            }

            var data = db.order_detail.Where(m => m.rowid == model.rowid).FirstOrDefault();
            data.product_no = model.product_no;
            data.workoder_no = model.workoder_no;
            data.remark = model.remark;
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult DeleteMaster(int id)
        {
            var model1 = db.order.Where(m => m.rowid == id).FirstOrDefault();

            //先刪除明細
            var model2 = db.order_detail
                .Where(m => m.order_no == model1.order_no).ToList();
            foreach (var item in model2)
            {
                db.order_detail.Remove(item);
            }
            db.SaveChanges();

            //再刪除表頭
            db.order.Remove(model1);
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
            var model = db.order_detail.Where(m => m.rowid == id).FirstOrDefault();
            if (model != null)
            {
                db.order_detail.Remove(model);
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

    }
}