using KoalaBlog.WebApi.Core.Managers;
using System.Threading.Tasks;
using System.Web.Http;

namespace KoalaBlog.WebApi.Controllers
{
    [RoutePrefix("koala/api/person")]
    public class PersonController : BaseApiController
    {
        // GET koala/api/person/GetPersonInfo/personId
        [Route("GetPersonInfo/{personId}"), HttpGet]
        public async Task<IHttpActionResult> GetPersonInfoAsync(long personId)
        {
            var result = await new PersonManager().GetPersonInfoAsync(personId);

            return Ok(result);
        }

        // GET koala/api/person/Follow/followerId/followingId
        [Route("Follow/{followerId}/{followingId}"), HttpGet]
        public async Task<IHttpActionResult> FollowAsync(long followerId, long followingId)
        {
            var result = await new PersonManager().FollowAsync(followerId, followingId);

            return Ok(result);
        }
    }
}