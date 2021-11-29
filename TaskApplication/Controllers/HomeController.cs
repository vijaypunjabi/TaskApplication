using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskApplication.InfraForAuthentication;

namespace TaskApplication.Controllers
{
    public class HomeController : Controller
    {
        [CustomAuthorizationFilter("Normal", "Admin")]
        public ActionResult Index()
        {
            return View();
        }
        [CustomAuthorizationFilter("Admin")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Unauthorized()
        {
            ViewBag.Message = "Unauthorized Page!";
            return View();
        }
    }
}