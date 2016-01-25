using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using KoalaBlog.WebModels;
using KoalaBlog.Framework.Extensions;

namespace KoalaBlog.ApiClient
{
    public class BlogClient : BaseClient
    {
        public BlogClient(Uri baseEndpoint)
            : base(baseEndpoint)
        {
        }
        
        public async Task<Blog> CreateBlogAsync(long personId, string content, int accessInfo, long? groupId = null, List<long> attachContentIds = null, long? forwardBlogId = null)
        {
            var postModel = new
            {
                PersonID = personId,
                ForwardBlogID = forwardBlogId,
                GroupID = groupId,
                Content = content,
                AccessInfo = accessInfo,
                AttachContentIds = attachContentIds
            };

            return await PostAsync<Blog>(RelativePaths.CreateBlog, postModel);
        }

        public async Task<List<Blog>> GetBlogsAsync(long personId, int pageIndex = 1, int pageSize = 10, long? groupId = null)
        {
            return await GetAsync<List<Blog>>(RelativePaths.GetBlogs.Link(personId, pageIndex, pageSize, groupId));
        }

        public async Task<List<Blog>> GetOwnBlogsAsync(long personId, int pageIndex = 1, int pageSize = 10)
        {
            return await GetAsync<List<Blog>>(RelativePaths.GetOwnBlogs.Link(personId, pageIndex, pageSize));
        }

        public async Task<Tuple<bool, int>> LikeAsync(long personId, long blogId)
        {
            return await GetAsync<Tuple<bool, int>>(RelativePaths.Like.Link(personId, blogId));
        }

        public async Task<bool> CollectAsync(long personId, long blogId)
        {
            var postModel = new
            {
                PersonID = personId,
                BlogID = blogId
            };

            return await PostAsync<bool>(RelativePaths.Collect, postModel);
        }

        protected class RelativePaths
        {
            private const string Prefix = "koala/api/blog";

            public const string CreateBlog = Prefix + "/CreateBlog";
            public const string GetBlogs = Prefix + "/GetBlogs";
            public const string GetOwnBlogs = Prefix + "/GetOwnBlogs";
            public const string Like = Prefix + "/Like";
            public const string Collect = Prefix + "/Collect";
        }
    }
}
