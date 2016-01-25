using KoalaBlog.WebApi.Core.Managers;
using KoalaBlog.WebApi.Models;
using System.Threading.Tasks;
using System.Web.Http;

namespace KoalaBlog.WebApi.Controllers
{
    [RoutePrefix("koala/api/comment")]
    public class CommentController : BaseApiController
    {
        [Route("AddComment"), HttpPost]
        public async Task<IHttpActionResult> AddCommentAsync([FromBody]AddCommentBindingModel model)
        {
            var result = await new CommentManager().AddCommentAsync(model.PersonID, model.BlogID, model.Content, model.PhotoContentIds, model.BaseCommentID);

            return Ok(result);
        }

        [Route("GetComments/{blogId:long}/{pageIndex:int=1}/{pageSize:int=15}"), HttpGet]
        public async Task<IHttpActionResult> GetCommentsAsync(long blogId, int pageIndex, int pageSize)
        {
            var result = await new CommentManager().GetCommentsAsync(blogId, pageIndex, pageSize);

            return Ok(result);
        }

        [Route("Like/{personId:long}/{commentId:long}"), HttpGet]
        public async Task<IHttpActionResult> LikeAsync(long personId, long commentId)
        {
            var result = await new CommentManager().LikeAsync(personId, commentId);

            return Ok(result);
        }
    }
}