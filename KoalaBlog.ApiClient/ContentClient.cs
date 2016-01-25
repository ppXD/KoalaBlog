using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using KoalaBlog.WebModels;
using KoalaBlog.Framework.Extensions;
using System.Web;

namespace KoalaBlog.ApiClient
{
    public class ContentClient : BaseClient
    {
        public ContentClient(Uri baseEndpoint)
            : base(baseEndpoint)
        {
        }

        public async Task<List<Content>> UploadImage(HttpPostedFileBase image)
        {
            return await PostFileAsync<List<Content>>(RelativePaths.UploadImage, image);
        }

        public async Task DeleteImage(long contentId)
        {
            await GetAsync(RelativePaths.DeleteImage.Link(contentId));
        }

        public async Task<Tuple<bool, List<Content>>> GetOwnContents(long personId, int pageIndex = 1, int pageSize = 10)
        {
            return await GetAsync<Tuple<bool, List<Content>>>(RelativePaths.GetOwnContents.Link(personId, pageIndex, pageSize));
        }

        protected class RelativePaths
        {
            private const string Prefix = "koala/api/content";

            public const string UploadImage = Prefix + "/UploadImage";
            public const string DeleteImage = Prefix + "/DeleteImage";
            public const string GetOwnContents = Prefix + "/GetOwnsContents";
        }
    }
}
