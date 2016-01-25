using KoalaBlog.Entity.Models;
using KoalaBlog.Framework.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KoalaBlog.Web.Controllers
{
    [RoutePrefix("profile")]
    public class PersonController : BaseController
    {
        [Route("shared"), HttpGet]
        public ActionResult ProfileShared()
        {
            return View();
        }

        [Route("photos"), HttpGet]
        public ActionResult ProfilePhotos()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetPersonInfo()
        {
            var result = await Services.PersonClient.GetPersonInfoAsync(CurrentThreadIdentityObject.PersonID);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}