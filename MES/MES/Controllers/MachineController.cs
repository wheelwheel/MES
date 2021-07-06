using MES.App_Class;
using MES.Models;
using MES.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace MES.Controllers
{
    public class MachineController : Controller
    {

        public IRepository<machine> repo_machine { get; set; }


        public MachineController()
        {
            repo_machine = new EFGenericRepository<machine>(new MESEntities());
        }

        public ActionResult GetDataList()
        {
            var datas = repo_machine.ReadAll().OrderBy(m => m.m_No);
            return Json(new { data = datas }, JsonRequestBehavior.AllowGet);
        }

        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult List()
        {
            return View();
        }
    }
}