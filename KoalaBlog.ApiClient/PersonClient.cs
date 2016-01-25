using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoalaBlog.Framework.Extensions;
using KoalaBlog.WebModels;

namespace KoalaBlog.ApiClient
{
    public class PersonClient : BaseClient
    {
        public PersonClient(Uri baseEndpoint)
            : base(baseEndpoint)
        {
        }

        public async Task<Person> GetPersonInfoAsync(long personId)
        {
            var result = await GetAsync<Person>(RelativePaths.GetPersonInfo.Link(personId));

            return result;
        }

        public async Task<bool> FollowAsync(long followerId, long followingId)
        {
            var result = await GetAsync<bool>(RelativePaths.Follow.Link(followerId, followingId));

            return result;
        }

        protected class RelativePaths
        {
            private const string Prefix = "koala/api/person";

            public const string GetPersonInfo = Prefix + "/GetPersonInfo";
            public const string Follow = Prefix + "/Follow";
        }
    }
}
