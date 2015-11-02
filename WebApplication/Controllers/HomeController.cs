using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
           // ServiceWebRoleReference.WOD wod = GetWOD()[0];

            return View();
        }
        

        public async Task<JsonResult> QueryServiceWebRole(string DateEx = "")
        {
            //ViewBag.Message = "Your application description page.";

            ServiceWebRoleReference.WOD[] wod = await GetWOD(DateEx);

            return Json(wod, JsonRequestBehavior.AllowGet);
        }
        
        private async Task<ServiceWebRoleReference.WOD[]> GetWOD(string DateEx = "")
        {
            return await service.GetWODAsync(DateEx);
        }
    }
}