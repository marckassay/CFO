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
            ServiceWebRoleReference.WOD wod = GetWOD()[0];

            return View();
        }
        
        public ActionResult QueryServiceWebRole(string DateEx = "")
        {
            //ViewBag.Message = "Your application description page.";

            ServiceWebRoleReference.WOD[] wod = GetWOD(DateEx);

            return View();
        }
        
        private ServiceWebRoleReference.WOD[] GetWOD(string DateEx = "")
        {
            return service.GetWOD(DateEx);
        }
    }
}