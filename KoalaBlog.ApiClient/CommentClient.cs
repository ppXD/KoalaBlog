using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using KoalaBlog.WebModels.Models;
using KoalaBlog.Framework.Extensions;

namespace KoalaBlog.ApiClient
{
    public class CommentClient : BaseClient
    {
        public CommentClient(Uri baseEndpoint)
            : base(baseEndpoint)
        {

        }

        public async Task<Comment> AddCommentAsync(long personId, long blogId, string content, List<long> photoContentIds = null, long? baseCommentId = null)
        {
            var postModel = new
            {
                PersonID = personId,
                BlogID = blogId,
                Content = content,
                PhotoContentIds = photoContentIds,
                BaseCommentID = baseCommentId
            };

            return await PostAsync<Comment>(RelativePaths.AddComment, postModel);
        }

        public async Task<Tuple<int, List<Comment>>> GetCommentsAsync(long blogId, int pageIndex = 1, int pageSize = 15)
        {
            return await GetAsync<Tuple<int, List<Comment>>>(RelativePaths.GetComments.Link(blogId, pageIndex, pageSize));
        }

        public async Task<Tuple<bool, int>> LikeAsync(long personId, long commentId)
        {
            return await GetAsync<Tuple<bool, int>>(RelativePaths.Like.Link(personId, commentId));
        }

        protected class RelativePaths
        {
            private const string Prefix = "koala/api/comment";

            public const string Like = Prefix + "/Like";
            public const string AddComment = Prefix + "/AddComment";
            public const string GetComments = Prefix + "/GetComments";
        }
    }
}
