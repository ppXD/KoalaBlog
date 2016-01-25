using KoalaBlog.WebApi.Core.Managers;
using KoalaBlog.WebApi.Models;
using System.Threading.Tasks;
using System.Web.Http;

namespace KoalaBlog.WebApi.Controllers
{
    [RoutePrefix("koala/api/blog")]
    public class BlogController : BaseApiController
    {
        [Route("GetBlogs/{personId:long}/{pageIndex:int=1}/{pageSize:int=10}/{groupId:long?}"), HttpGet]
        public async Task<IHttpActionResult> GetBlogsAsync(long personId, int pageIndex, int pageSize, long? groupId = null)
        {
            var result = await new BlogManager().GetBlogsAsync(personId, groupId, pageIndex, pageSize);

            return Ok(result);
        }

        [Route("GetOwnBlogs/{personId:long}/{pageIndex:int=1}/{pageSize:int=10}"), HttpGet]
        public async Task<IHttpActionResult> GetOwnBlogs(long personId, int pageIndex, int pageSize)
        {
            var result = await new BlogManager().GetOwnBlogs(personId, pageIndex, pageSize);

            return Ok(result);
        }

        [Route("CreateBlog"), HttpPost]
        public async Task<IHttpActionResult> CreateBlogAsync([FromBody]CreateBlogBindingModel model)
        {
            var result = await new BlogManager().CreateBlogAsync(model.PersonID, model.Content, model.AccessInfo, model.GroupID, model.AttachContentIds, model.ForwardBlogID);

            return Ok(result);
        }

        [Route("Collect"), HttpPost]
        public async Task<IHttpActionResult> CollectAsync([FromBody]CollectBindingModel model)
        {
            var result = await new BlogManager().CollectAsync(model.PersonID, model.BlogID);

            return Ok(result);
        }

        [Route("Like/{personId:long}/{blogId:long}"), HttpGet]
        public async Task<IHttpActionResult> LikeAsync(long personId, long blogId)
        {
            var result = await new BlogManager().LikeAsync(personId, blogId);

            return Ok(result);
        }
    }
}