using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MES.Controllers
{
    public class HomeController : Controller
    {
        //[LoginAuthorize(RoleList ="User,Admin")]
        public ActionResult Index()
        {
            return View();
        }

        //[LoginAuthorize(RoleList = "User")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        //[LoginAuthorize(RoleList = "User")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}