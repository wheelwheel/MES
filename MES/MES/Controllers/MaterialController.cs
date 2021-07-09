using MES.App_Class;
using MES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace MES.Controllers
{
    public class MaterialController : Controller
    {
        public IRepository<material> repo_material { get; set; }

        public MaterialController()
        {
            repo_material = new EFGenericRepository<material>(new MESEntities());
        }

        public ActionResult GetDataList()
        {
            var datas = repo_material.ReadAll().OrderBy(m => m.m_No);
            return Json(new { data = datas }, JsonRequestBehavior.AllowGet);
        }

        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult List()
        {
            return View();
        }


        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult Infor(MaterialViewModel model)
        {
            using (MESEntities db = new MESEntities())
            {
                var datas = db.material
                    .Where(m => m.m_No == MaterialSearch.MNO).ToList();
                return View(datas);
            }
        }
        // GET: Material
        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EnTraceCode()
        {
            MaterialViewModel model = new MaterialViewModel()
            {
                mNo = ""
            };
            return View(model);
        }

        [LoginAuthorize(RoleList = "User,Admin")]
        [HttpPost]
        public ActionResult EnTraceCode(MaterialViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            using (MESEntities db = new MESEntities())
            {
                var data = db.material
                    .Where(m => m.m_No == model.mNo)
                    .FirstOrDefault();
                if (data == null)
                {
                    ModelState.AddModelError("mNo", "物料編號錯誤");
                    return View(model);
                }
                MaterialSearch.MNO = model.mNo;
                return RedirectToAction("Infor", "Material");

            }
        }

        //新增
        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult Create()
        {
            material model = new material()
            {
                m_No = "",
                m_Name = "",
                factory_no = "",
                shape_no = "",
                shape_length = 0,
                shape_diameter = 0,
                length = 0,
                remark = ""
            };
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult Create(material model)
        {
            if (!ModelState.IsValid) return View(model);
            using (MESEntities db = new MESEntities())
            {
                if (!ModelState.IsValid) return View(model);
                bool bln_error = false;
                var check = db.material.Where(m => m.m_No == model.m_No).FirstOrDefault();
                if (check != null) { ModelState.AddModelError("", "物料編號重複!"); bln_error = true; }
                if (bln_error) return View(model);


                material data = new material()
                {
                    m_No = model.m_No,
                    m_Name = model.m_Name,
                    factory_no = model.factory_no,
                    shape_no = model.shape_no,
                    shape_length = model.shape_length,
                    shape_diameter = model.shape_diameter,
                    length = model.length,
                    remark = model.remark
                };

                db.material.Add(data);
                db.SaveChanges();

                return RedirectToAction("List");
            }
        }
    }
}