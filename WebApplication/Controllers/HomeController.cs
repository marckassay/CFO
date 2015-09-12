using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private ServiceWebRoleReference.ServiceClient service;

        public HomeController()
        {
            service = new ServiceWebRoleReference.ServiceClient();
        }

        public ActionResult Index()
        {
            ServiceWebRoleReference.WOD[] wod = service.GetWOD("");

            return View();
        }

        public ActionResult About()
        {
            //ViewBag.Message = "Your application description page.";



            return View();
        }
    }
}