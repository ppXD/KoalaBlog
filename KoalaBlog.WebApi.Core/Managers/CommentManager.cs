using KoalaBlog.BLL.Handlers;
using KoalaBlog.DAL;
using KoalaBlog.DTOs;
using KoalaBlog.DTOs.Converters;
using KoalaBlog.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.WebApi.Core.Managers
{
    public class CommentManager : ManagerBase
    {
        /// <summary>
        /// 发表评论
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="blogId">BlogID</param>
        /// <param name="content">评论内容</param>
        /// <param name="photoContentIds">评论附带图片的ID集合</param>
        /// <param name="baseCommentId">被评论的CommentID</param>
        /// <returns></returns>
        public async Task<CommentDTO> AddCommentAsync(long personId, long blogId, string content, List<long> photoContentIds = null, long? baseCommentId = null)
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                PersonHandler perHandler = new PersonHandler(dbContext);
                AvatarHandler avatarHandler = new AvatarHandler(dbContext);
                CommentHandler commentHandler = new CommentHandler(dbContext);

                //1. 发表评论并返回Comment Entity对象。
                var comment = await commentHandler.AddCommentAsync(personId, blogId, content, photoContentIds, baseCommentId);

                //2. 将Entity对象Convert为DTO对象。
                var commentDto = comment.ToDTO();

                //3. 判断Person对象是否为空，如果为空则获取。
                if (commentDto.Person == null)
                {
                    var personEntity = await perHandler.GetByIdAsync(comment.PersonID);

                    if (personEntity != null)
                    {
                        commentDto.Person = personEntity.ToDTO();

                        //3.1 判断头像Url是否已经获取。
                        if (string.IsNullOrEmpty(commentDto.Person.AvatarUrl))
                        {
                            commentDto.Person.AvatarUrl = await avatarHandler.GetActiveAvatarUrlByPersonId(comment.PersonID);
                        }
                    }
                }
                else
                {
                    //3.2 如果Person对象不为空，判断头像Url是否已经获取。
                    if (string.IsNullOrEmpty(commentDto.Person.AvatarUrl))
                    {
                        commentDto.Person.AvatarUrl = await avatarHandler.GetActiveAvatarUrlByPersonId(comment.PersonID);
                    }
                }

                //4. 判断此评论是否评论了其他的评论。
                if(comment.NewCommentXComments != null && comment.NewCommentXComments.Count > 0)
                {
                    commentDto.BaseComment = comment.NewCommentXComments.First().BaseComment.ToDTO();

                    //4.1 判断Person对象是否为空，如果为空则获取，这里暂时不需要获取Avatar。
                    if (commentDto.BaseComment.Person == null)
                    {
                        var personEntity = await perHandler.GetByIdAsync(comment.NewCommentXComments.First().BaseComment.PersonID);

                        if (personEntity != null)
                        {
                            commentDto.BaseComment.Person = personEntity.ToDTO();
                        }
                    }
                }

                return commentDto;
            }
        }

        /// <summary>
        /// 获取Blog的评论
        /// </summary>
        /// <param name="blogId">BlogID</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<Tuple<int, List<CommentDTO>>> GetCommentsAsync(long blogId, int pageIndex, int pageSize)
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                int totalCommentCount = 0;

                List<CommentDTO> commentDtoList = null;

                PersonHandler perHandler = new PersonHandler(dbContext);
                AvatarHandler avatarHandler = new AvatarHandler(dbContext);
                CommentHandler commentHandler = new CommentHandler(dbContext);
                CommentXContentHandler cxContentHandler = new CommentXContentHandler(dbContext);
                CommentXCommentHandler cxCommentHandler = new CommentXCommentHandler(dbContext);

                //1. 获取Blog评论列表。
                var comments = await commentHandler.GetCommentsAsync(blogId, pageIndex, pageSize);

                if (comments.Count > 0)
                {
                    commentDtoList = new List<CommentDTO>();

                    foreach (var comment in comments)
                    {
                        CommentDTO commentDto = comment.ToDTO();

                        //2. 判断Person对象是否为空，如果为空则获取。
                        if (commentDto.Person == null)
                        {
                            var personEntity = await perHandler.GetByIdAsync(comment.PersonID);

                            if (personEntity != null)
                            {
                                commentDto.Person = personEntity.ToDTO();

                                //2.1 判断头像Url是否已经获取。
                                if (string.IsNullOrEmpty(commentDto.Person.AvatarUrl))
                                {
                                    commentDto.Person.AvatarUrl = await avatarHandler.GetActiveAvatarUrlByPersonId(comment.PersonID);
                                }
                            }
                        }
                        else
                        {
                            //2.2 如果Person对象不为空，判断头像Url是否已经获取。
                            if (string.IsNullOrEmpty(commentDto.Person.AvatarUrl))
                            {
                                commentDto.Person.AvatarUrl = await avatarHandler.GetActiveAvatarUrlByPersonId(comment.PersonID);
                            }
                        }

                        //3. 判断Contents集合是否为空，如果为空则获取。
                        if (commentDto.Contents == null)
                        {
                            List<Content> contentList = await cxContentHandler.GetContentsAsync(comment.ID);

                            if (contentList != null && contentList.Count > 0)
                            {
                                commentDto.Contents = new List<ContentDTO>();

                                foreach (var content in contentList)
                                {
                                    ContentDTO contentDto = content.ToDTO();

                                    commentDto.Contents.Add(contentDto);
                                }
                            }
                        }

                        //4. 判断此评论是否评论了其他的评论。
                        if (commentDto.BaseComment == null)
                        {
                            Comment baseComment = await cxCommentHandler.GetBaseCommentByCommentIdAsync(comment.ID);

                            if (baseComment != null)
                            {
                                commentDto.BaseComment = baseComment.ToDTO();

                                //4.1 判断Person对象是否为空，如果为空则获取，这里暂时不需要获取Avatar。
                                if (commentDto.BaseComment.Person == null)
                                {
                                    var personEntity = await perHandler.GetByIdAsync(baseComment.PersonID);

                                    if (personEntity != null)
                                    {
                                        commentDto.BaseComment.Person = personEntity.ToDTO();
                                    }
                                }
                            }
                        }

                        //5. 获取评论点赞数量Task。
                        Task<int> likeCountTask = GetCommentLikeCountAsync(comment.ID);

                        //6. 判断用户是否点赞了此评论Task。
                        Task<bool> isLikeTask = IsLikeAsync(CurrentThreadIdentityObject.PersonID, comment.ID);

                        commentDto.IsLike = await isLikeTask;
                        commentDto.LikeCount = await likeCountTask;

                        commentDtoList.Add(commentDto);
                    }

                    //7. 获取Blog的总评论数量。
                    totalCommentCount = await GetCommentCountAsync(blogId);
                }

                return new Tuple<int, List<CommentDTO>>(totalCommentCount, commentDtoList);
            }
        }

        /// <summary>
        /// 获取评论的Content集合
        /// </summary>
        /// <param name="commentId">CommentID</param>
        /// <returns></returns>
        public async Task<List<Content>> GetCommentContentsAsync(long commentId)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                CommentXContentHandler cxcHandler = new CommentXContentHandler(dbContext);

                return await cxcHandler.GetContentsAsync(commentId);
            }
        }

        /// <summary>
        /// 获取评论的点赞数量
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task<int> GetCommentLikeCountAsync(long commentId)
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                EntityLikeHandler likeHandler = new EntityLikeHandler(dbContext);

                return await likeHandler.GetCommentLikeCountAsync(commentId);
            }
        }

        /// <summary>
        /// 判断用户是否点赞了此评论
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="commentId">CommentID</param>
        /// <returns></returns>
        public async Task<bool> IsLikeAsync(long personId, long commentId)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                EntityLikeHandler likeHandler = new EntityLikeHandler(dbContext);

                return await likeHandler.IsLikeAsync(personId, commentId, typeof(Comment));
            }
        }

        /// <summary>
        /// 获取Blog评论的数量
        /// </summary>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<int> GetCommentCountAsync(long blogId)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                CommentHandler commentHandler = new CommentHandler(dbContext);

                return await commentHandler.GetCommentCountAsync(blogId);
            }
        }

        /// <summary>
        /// Comment点赞或者取消赞
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<Tuple<bool, int>> LikeAsync(long personId, long commentId)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                EntityLikeHandler entityLikeHandler = new EntityLikeHandler(dbContext);

                return await entityLikeHandler.LikeAsync(personId, commentId, typeof(Comment));
            }
        }
    }
}
