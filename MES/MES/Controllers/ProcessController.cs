using MES.Models;
using MES.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace MES.Controllers
{
    public class ProcessController : Controller
    {
        MESEntities db = new MESEntities();

        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult List(int page=0)
        {
            if (page > 0) AppSession.MasterPage = page;
            AppSession.MasterPageSize = 5;
            ProcessViewModel model = new ProcessViewModel();
            model.processList = db.process.OrderBy(m => m.proc_no)
                .ToPagedList(AppSession.MasterPage, AppSession.MasterPageSize);
            return View(model);
        }

        public ActionResult MasterPage(int page = 1)
        {
            AppSession.MasterPage = page;
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult Create()
        {

            process model = new process()
            {
                proc_no = "",
                proc_name = ""
            };
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult Create(process model)
        {
            if (!ModelState.IsValid) return View(model);
            bool bln_error = false;
            var check1 = db.process.Where(m => m.proc_no == model.proc_no).FirstOrDefault();
            if (check1 != null) { ModelState.AddModelError("proc_no", "編號重複"); bln_error = true; }
            var check2 = db.process.Where(m => m.proc_name == model.proc_name).FirstOrDefault();
            if (check2 != null) { ModelState.AddModelError("proc_name", "名稱重複!"); bln_error = true; }
            if (bln_error) return View(model);

            db.process.Add(model);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult Edit(int id)
        {
            var model = db.process.Where(m => m.rowid == id).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult Edit(process model)
        {
            if (!ModelState.IsValid) return View(model);
            var data = db.process.Where(m => m.rowid == model.rowid).FirstOrDefault();
            data.proc_no = model.proc_no;
            data.proc_name = model.proc_name;
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult Delete(int id)
        {
            var model = db.process.Where(m => m.rowid == id).FirstOrDefault();
            if (model != null)
            {
                db.process.Remove(model);
                db.SaveChanges();
            }
            if (AppSession.MasterPage > 1) AppSession.MasterPage--;
            return RedirectToAction("List");
        }

       
    }
}