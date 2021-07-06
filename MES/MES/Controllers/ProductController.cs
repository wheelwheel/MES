using MES.App_Class;
using MES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MES.Models.ViewModel;
using PagedList;

namespace MES.Controllers
{
    public class ProductController : Controller
    {
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult GetDataList()
        {
            using (MESEntities db = new MESEntities())
            {
                var datas = db.re_complete_detail
                  .Join(db.re_complete, p => p.re_complete_no, d => d.re_complete_no,
                    (p1, d1) => new
                    {
                        re_complete_no = p1.re_complete_no,
                        order_no = d1.order_no,
                        proc_no = p1.proc_no,
                        //proc_name = p1.proc_name,
                        mach_no = p1.mach_no,
                        //mach_name = p1.mach_name,
                        user_no = p1.user_no,
                        start_time = p1.start_time,
                        end_time = p1.end_time,
                        qty = p1.qty
                    })
                    .Join(db.process, p => p.proc_no, d => d.proc_no,
                    (p2, d2) => new { p2, proc_name = d2.proc_name })
                    .Join(db.machine,p=>p.p2.mach_no,d=>d.m_No,
                    (p3,d3)=>new {p3,mach_name=d3.m_Name })
                    .Join(db.order_detail, p => p.p3.p2.order_no, d => d.order_no,
                    (p4, d4) => new { p4, product_no = d4.product_no })
                   .Join(db.product, p => p.product_no, d => d.product_no,
                    (p5, d5) => new { p5, product_name = d5.product_name })
                   .ToList();

                List<ProductViewModel> data = new List<ProductViewModel>();
                foreach (var item in datas)
                {
                    ProductViewModel d = new ProductViewModel();
                    d.re_complete_no = item.p5.p4.p3.p2.re_complete_no;
                    d.order_no = item.p5.p4.p3.p2.order_no;
                    d.product_no = item.p5.product_no;
                    d.product_name = item.product_name;
                    d.proc_no = item.p5.p4.p3.p2.proc_no;
                    d.proc_name = item.p5.p4.p3.proc_name;
                    d.mach_no = item.p5.p4.p3.p2.mach_no;
                    d.mach_name = item.p5.p4.mach_name;
                    d.user_no = item.p5.p4.p3.p2.user_no;
                    d.start_time = item.p5.p4.p3.p2.start_time;
                    d.end_time = item.p5.p4.p3.p2.end_time;
                    d.qty = item.p5.p4.p3.p2.qty;
                    data.Add(d);
                }

                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
        }

        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult List()
        {
            return View();
        }



        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult Infor()
        {
            using (MESEntities db = new MESEntities())
            {
                string orderNo = Session["order_no"].ToString();
                var datas = db.re_complete_detail
                    .Join(db.re_complete, p => p.re_complete_no, d => d.re_complete_no,
                    (p1, d1) => new
                    {
                        re_complete_no = p1.re_complete_no,
                        order_no = d1.order_no,
                        proc_no = p1.proc_no,
                        //proc_name = p1.proc_name,
                        mach_no = p1.mach_no,
                        //mach_name = p1.mach_name,
                        user_no = p1.user_no,
                        start_time = p1.start_time,
                        end_time = p1.end_time,
                        qty = p1.qty
                    })
                     .Join(db.process, p => p.proc_no, d => d.proc_no,
                    (p2, d2) => new { p2, proc_name = d2.proc_name })
                    .Join(db.machine, p => p.p2.mach_no, d => d.m_No,
                    (p3, d3) => new { p3, mach_name = d3.m_Name })
                    .Join(db.order_detail, p => p.p3.p2.order_no, d => d.order_no,
                    (p4, d4) => new { p4, product_no = d4.product_no })
                   .Join(db.product, p => p.product_no, d => d.product_no,
                    (p5, d5) => new { p5, product_name = d5.product_name })
                    .Where(m => m.p5.p4.p3.p2.order_no == orderNo)
                    .ToList();


                if (datas == null || datas.Count == 0)
                {
                    return RedirectToAction("EnTraceCode", "Product");
                }

                List<ProductViewModel> data = new List<ProductViewModel>();
                foreach (var item in datas)
                {
                    ProductViewModel d = new ProductViewModel();
                    d.re_complete_no = item.p5.p4.p3.p2.re_complete_no;
                    d.order_no = item.p5.p4.p3.p2.order_no;
                    d.product_no = item.p5.product_no;
                    d.product_name = item.product_name;
                    d.proc_no = item.p5.p4.p3.p2.proc_no;
                    d.proc_name = item.p5.p4.p3.proc_name;
                    d.mach_no = item.p5.p4.p3.p2.mach_no;
                    d.mach_name = item.p5.p4.mach_name;
                    d.user_no = item.p5.p4.p3.p2.user_no;
                    d.start_time = item.p5.p4.p3.p2.start_time;
                    d.end_time = item.p5.p4.p3.p2.end_time;
                    d.qty = item.p5.p4.p3.p2.qty;
                    data.Add(d);
                }
                return View(data);
            }
        }



        [HttpGet]

        public ActionResult InforForGuest()
        {
            using (MESEntities db = new MESEntities())
            {
                string orderNo = Session["order_no"].ToString();
                //string orderNo = "20201028001";
                var datas = db.re_complete_detail
                    .Join(db.re_complete, p => p.re_complete_no, d => d.re_complete_no,
                    (p1, d1) => new
                    {
                        re_complete_no = p1.re_complete_no,
                        order_no = d1.order_no,
                        proc_no = p1.proc_no,
                        //proc_name = p1.proc_name,
                        mach_no = p1.mach_no,
                        //mach_name = p1.mach_name,
                        user_no = p1.user_no,
                        start_time = p1.start_time,
                        end_time = p1.end_time,
                        qty = p1.qty
                    })
                    .Join(db.process, p => p.proc_no, d => d.proc_no,
                    (p2, d2) => new { p2, proc_name = d2.proc_name })
                    .Join(db.machine, p => p.p2.mach_no, d => d.m_No,
                    (p3, d3) => new { p3, mach_name = d3.m_Name })
                    .Join(db.order_detail, p => p.p3.p2.order_no, d => d.order_no,
                    (p4, d4) => new { p4, product_no = d4.product_no })
                    .Join(db.product, p => p.product_no, d => d.product_no,
                    (p5, d5) => new
                    {
                        p5,
                        product_name = d5.product_name,
                        Diameter = d5.Diameter,
                        Length = d5.Length,
                        Diameter_Tolerance = d5.Diameter_Tolerance,
                        Straightness = d5.Straightness
                    })
                    .Where(m => m.p5.p4.p3.p2.order_no == orderNo)
                    .ToList();

                if (datas == null || datas.Count == 0)
                {
                    return RedirectToAction("EnTraceCode", "Product");
                }



                List<ProductViewModel> data = new List<ProductViewModel>();
                foreach (var item in datas)
                {
                    ProductViewModel d = new ProductViewModel();
                    //d.re_complete_no = item.p3.p2.re_complete_no;
                    d.order_no = item.p5.p4.p3.p2.order_no;
                    d.product_no = item.p5.product_no;
                    d.product_name = item.product_name;
                    d.proc_no = item.p5.p4.p3.p2.proc_no;
                    d.proc_name = item.p5.p4.p3.proc_name;
                    d.mach_no = item.p5.p4.p3.p2.mach_no;
                    d.mach_name = item.p5.p4.mach_name;
                    d.Diameter = item.Diameter;
                    d.Length = item.Length;
                    d.Diameter_Tolerance = item.Diameter_Tolerance;
                    d.Straightness = item.Straightness;
                    d.user_no = item.p5.p4.p3.p2.user_no;
                    d.start_time = item.p5.p4.p3.p2.start_time;
                    d.end_time = item.p5.p4.p3.p2.end_time;
                    d.qty = item.p5.p4.p3.p2.qty;
                    data.Add(d);
                }
                return View(data);
            }
        }

        [HttpGet]
        public ActionResult EnTraceCode()
        {
            ProductViewModel model = new ProductViewModel()
            {
                order_no = ""
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EnTraceCode(ProductViewModel model)
        {
            if (!ModelState.IsValid) return View(model);


            if (model.order_no == null)
            {
                ModelState.AddModelError("order_no", "無資料");
                return View(model);
            }
            else
            {
                Session["order_no"] = model.order_no;
            }

            if (UserAccount.IsLogin == true)
            {
                return RedirectToAction("Infor", "Product");
            }
            else
            {
                return RedirectToAction("InforForGuest", "Product");
            }
        }
    }
}