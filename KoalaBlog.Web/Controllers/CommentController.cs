using KoalaBlog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KoalaBlog.Web.Controllers
{
    public class CommentController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> GetComments(long blogId)
        {
            var result = await Services.CommentClient.GetCommentsAsync(blogId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> AddComment(AddCommentViewModel model)
        {
            var result = await Services.CommentClient.AddCommentAsync(CurrentThreadIdentityObject.PersonID, model.BlogID, model.Content, model.photoContentIds, model.BaseCommentID);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Like(long commentId)
        {
            var result = await Services.CommentClient.LikeAsync(CurrentThreadIdentityObject.PersonID, commentId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}