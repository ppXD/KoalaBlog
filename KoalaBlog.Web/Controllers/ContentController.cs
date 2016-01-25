using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KoalaBlog.Web.Controllers
{
    public class ContentController : BaseController
    {
        public async Task<ActionResult> UploadImage(HttpPostedFileBase image)
        {
            var result = await Services.ContentClient.UploadImage(image);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> DeleteImage(long contentId)
        {
            await Services.ContentClient.DeleteImage(contentId);

            return new EmptyResult();
        }

        [HttpGet]
        public async Task<ActionResult> GetOwnContents(int pageIndex = 1)
        {
            var result = await Services.ContentClient.GetOwnContents(CurrentThreadIdentityObject.PersonID, pageIndex);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}