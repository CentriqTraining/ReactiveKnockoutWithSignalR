using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReactiveKnockoutVersion2.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Step1()
        {
            ViewBag.Active1 = true;
            return View();
        }
        public ActionResult Step2()
        {
            ViewBag.Active2 = true;
            return View();
        }
        public ActionResult Step3()
        {
            ViewBag.Active3 = true;
            return View();
        }
        public ActionResult Step4()
        {
            ViewBag.Active4 = true;
            return View();
        }
        public ActionResult Step5()
        {
            ViewBag.Active5 = true;
            return View();
        }
        public ActionResult Step6()
        {
            ViewBag.Active6 = true;
            return View();
        }
    }
}
