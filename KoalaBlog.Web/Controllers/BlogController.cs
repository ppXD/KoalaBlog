using KoalaBlog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KoalaBlog.Web.Controllers
{
    public class BlogController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult> CreateBlog(CreateBlogViewModel model)
        {
            var result = await Services.BlogClient.CreateBlogAsync(CurrentThreadIdentityObject.PersonID, model.Content, model.AccessInfo, model.GroupID, model.AttachContentIds, model.ForwardBlogID);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetBlogs(int pageIndex = 1)
        {
            var result = await Services.BlogClient.GetBlogsAsync(CurrentThreadIdentityObject.PersonID, pageIndex);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetOwnBlogs(int pageIndex = 1)
        {
            var result = await Services.BlogClient.GetOwnBlogsAsync(CurrentThreadIdentityObject.PersonID, pageIndex);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Like(long blogId)
        {
            var result = await Services.BlogClient.LikeAsync(CurrentThreadIdentityObject.PersonID, blogId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Collect(long blogId)
        {
            var result = await Services.BlogClient.CollectAsync(CurrentThreadIdentityObject.PersonID, blogId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}