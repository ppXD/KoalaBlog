using KoalaBlog.BLL.Handlers;
using KoalaBlog.DAL;
using KoalaBlog.DTOs;
using KoalaBlog.DTOs.Converters;
using KoalaBlog.Entity.Models;
using KoalaBlog.Entity.Models.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace KoalaBlog.WebApi.Core.Managers
{
    public class BlogManager : ManagerBase
    {
        /// <summary>
        /// 发布一篇Blog
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="content">Blog的内容</param>
        /// <param name="attachContents">Blog的附件</param>
        /// <param name="accessInfo">Blog的访问控制</param>
        /// <param name="groupId">当Blog的访问控制为群可见则需要指定GroupID</param>
        /// <param name="forwardBlogId">转发的BlogID</param>
        /// <returns></returns>
        public async Task<BlogDTO> CreateBlogAsync(long personId, string content, BlogInfoAccessInfo accessInfo, long? groupId = null, List<long> attachContentIds = null, long? forwardBlogId = null)
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogHandler blogHandler = new BlogHandler(dbContext);                
                PersonHandler perHandler = new PersonHandler(dbContext);
                AvatarHandler avatarHandler = new AvatarHandler(dbContext);

                //1. 发布Blog并返回Blog Entity对象。
                var blog = await blogHandler.CreateBlogAsync(personId, content, accessInfo, groupId, attachContentIds, forwardBlogId);

                //2. 将Entity对象Convert为DTO对象。
                var result = blog.ToDTO();

                //3. 判断Person对象是否为空，如果为空则获取。
                if(result.Person == null)
                {
                    var personEntity = await perHandler.GetByIdAsync(blog.PersonID);

                    if (personEntity != null)
                    {
                        result.Person = personEntity.ToDTO();

                        //3.1 判断头像Url是否已经获取。
                        if (string.IsNullOrEmpty(result.Person.AvatarUrl))
                        {
                            result.Person.AvatarUrl = await avatarHandler.GetActiveAvatarUrlByPersonId(result.Person.ID);
                        }
                    }
                }
                else
                {
                    //3.2 如果Person对象不为空，判断头像Url是否已经获取。
                    if (string.IsNullOrEmpty(result.Person.AvatarUrl))
                    {
                        result.Person.AvatarUrl = await avatarHandler.GetActiveAvatarUrlByPersonId(result.Person.ID);
                    }
                }

                //4. 判断是否转发了Blog，转发了则获取转发Blog的信息。
                if(blog.NewBlogXBlogs != null && blog.NewBlogXBlogs.Count > 0)
                {
                    var baseBlogXblog = blog.NewBlogXBlogs.SingleOrDefault(x => x.IsBase);

                    if(baseBlogXblog != null)
                    {
                        CommentHandler commentHandler = new CommentHandler(dbContext);
                        BlogXBlogHandler bxbHandler = new BlogXBlogHandler(dbContext);
                        EntityLikeHandler likeHandler = new EntityLikeHandler(dbContext);
                        BlogXContentHandler bxcHandler = new BlogXContentHandler(dbContext);

                        result.BaseBlog = baseBlogXblog.BaseBlog.ToDTO();

                        //4.1 判断转发了Blog的Person对象是否为空，如果空则获取。
                        if(result.BaseBlog.Person == null)
                        {
                            var personEntity = await perHandler.GetByIdAsync(baseBlogXblog.BaseBlog.PersonID);

                            if (personEntity != null)
                            {
                                result.BaseBlog.Person = personEntity.ToDTO();
                            }
                        }
                        //4.2 判断转发了Blog的是否有图片等等。
                        List<Content> contentList = await bxcHandler.GetContentsAsync(result.BaseBlog.ID);

                        if(contentList != null && contentList.Count > 0)
                        {
                            result.BaseBlog.Contents = new List<ContentDTO>();

                            foreach (var contentObj in contentList)
                            {
                                ContentDTO contentDto = contentObj.ToDTO();

                                result.BaseBlog.Contents.Add(contentDto);
                            }
                        }

                        //4.3 获取转发的Blog的转发数量。
                        result.BaseBlog.RepostCount = await bxbHandler.GetRepostCountAsync(result.BaseBlog.ID);

                        //4.4 获取转发的Blog的评论数量。
                        result.BaseBlog.CommentCount = await commentHandler.GetCommentCountAsync(result.BaseBlog.ID);

                        //4.5 获取转发的Blog的点赞数量
                        result.BaseBlog.LikeCount = await likeHandler.GetBlogLikeCountAsync(result.BaseBlog.ID);

                        //4.6 获取转发的Blog是否已经点赞。
                        result.BaseBlog.IsLike = await likeHandler.IsLikeAsync(personId, result.BaseBlog.ID, typeof(Blog));
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// 获取用户关注的人Blog
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="groupId">GroupID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">数量/页</param>
        /// <returns></returns>
        public async Task<List<BlogDTO>> GetBlogsAsync(long personId, long? groupId, int pageIndex, int pageSize)
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                List<BlogDTO> blogDtoList = null;

                BlogHandler blogHandler = new BlogHandler(dbContext);

                //1. 获取正在关注的人Blog集合。
                var blogs = await blogHandler.GetBlogsAsync(personId, groupId, pageIndex, pageSize);

                if (blogs.Count > 0)
                {
                    PersonHandler perHandler = new PersonHandler(dbContext);                    
                    AvatarHandler avatarHandler = new AvatarHandler(dbContext);
                    CommentHandler commentHandler = new CommentHandler(dbContext);
                    BlogXBlogHandler bxbHandler = new BlogXBlogHandler(dbContext);
                    EntityLikeHandler likeHandler = new EntityLikeHandler(dbContext);
                    BlogXContentHandler bxcHandler = new BlogXContentHandler(dbContext);

                    blogDtoList = new List<BlogDTO>();

                    foreach (var blog in blogs)
                    {
                        BlogDTO blogDto = blog.ToDTO();

                        //2. 判断Person对象是否为空，如果为空则获取。
                        if (blogDto.Person == null)
                        {
                            var personEntity = await perHandler.GetByIdAsync(blog.PersonID);

                            if (personEntity != null)
                            {
                                blogDto.Person = personEntity.ToDTO();

                                //3.1 判断头像Url是否已经获取。
                                if (string.IsNullOrEmpty(blogDto.Person.AvatarUrl))
                                {
                                    blogDto.Person.AvatarUrl = await avatarHandler.GetActiveAvatarUrlByPersonId(blogDto.Person.ID);
                                }
                            }
                        }
                        else
                        {
                            //2.2 如果Person对象不为空，判断头像Url是否已经获取。
                            if (string.IsNullOrEmpty(blogDto.Person.AvatarUrl))
                            {
                                blogDto.Person.AvatarUrl = await avatarHandler.GetActiveAvatarUrlByPersonId(blogDto.Person.ID);
                            }
                        }

                        //3. 判断Contents集合是否为空，如果为空则获取。
                        if (blogDto.Contents == null)
                        {
                            List<Content> contentList = await bxcHandler.GetContentsAsync(blogDto.ID);

                            if (contentList != null && contentList.Count > 0)
                            {
                                blogDto.Contents = new List<ContentDTO>();

                                foreach (var content in contentList)
                                {
                                    ContentDTO contentDto = content.ToDTO();

                                    blogDto.Contents.Add(contentDto);
                                }
                            }
                        }

                        //4. 判断此Blog是否转发了其他Blog。
                        if(blogDto.BaseBlog == null)
                        {
                            Blog baseBlog = await bxbHandler.GetBaseBlogByBlogIdAsync(blogDto.ID);

                            if(baseBlog != null)
                            {
                                blogDto.BaseBlog = baseBlog.ToDTO();

                                //4.1 判断转发的Blog的Person对象是否为空，如果为空则获取。不需要获取头像。
                                if (blogDto.BaseBlog.Person == null)
                                {
                                    var personEntity = await perHandler.GetByIdAsync(baseBlog.PersonID);

                                    if (personEntity != null)
                                    {
                                        blogDto.BaseBlog.Person = personEntity.ToDTO();
                                    }
                                }
                                //4.2 判断转发的Blog是否有发Contents。
                                if (blogDto.BaseBlog.Contents == null)
                                {
                                    List<Content> contentList = await bxcHandler.GetContentsAsync(blogDto.BaseBlog.ID);

                                    if (contentList != null && contentList.Count > 0)
                                    {
                                        blogDto.BaseBlog.Contents = new List<ContentDTO>();

                                        foreach (var content in contentList)
                                        {
                                            ContentDTO contentDto = content.ToDTO();

                                            blogDto.BaseBlog.Contents.Add(contentDto);
                                        }
                                    }
                                }

                                //4.3 获取转发的Blog的转发数量。
                                blogDto.BaseBlog.RepostCount = await bxbHandler.GetRepostCountAsync(blogDto.BaseBlog.ID);

                                //4.4 获取转发的Blog的评论数量。
                                blogDto.BaseBlog.CommentCount = await commentHandler.GetCommentCountAsync(blogDto.BaseBlog.ID);

                                //4.5 获取转发的Blog的点赞数量
                                blogDto.BaseBlog.LikeCount = await likeHandler.GetBlogLikeCountAsync(blogDto.BaseBlog.ID);

                                //4.6 获取转发的Blog是否已经点赞。
                                blogDto.BaseBlog.IsLike = await likeHandler.IsLikeAsync(personId, blogDto.BaseBlog.ID, typeof(Blog));
                            }
                        }

                        //5. 获取用户是否收藏了Blog的Task。
                        Task<bool> isFavoriteTask = IsFavoriteAsync(personId, blog.ID);

                        //6. 获取评论数量的Task。
                        CommentManager commentManager = new CommentManager();

                        Task<int> commentCountTask = commentManager.GetCommentCountAsync(blog.ID);

                        //7. 获取转发数量的Task。
                        Task<int> repostCountTask = GetRepostCountAsync(blog.ID);

                        //8. 获取点赞数量和用户是否已经点赞的Task。
                        Task<Tuple<int, bool>> likeObjTask = GetLikeObjectAsync(personId, blog.ID);

                        blogDto.IsFavorite = await isFavoriteTask;
                        blogDto.CommentCount = await commentCountTask;
                        blogDto.RepostCount = await repostCountTask;

                        //9. 获取点赞数量和用户是否点赞的对象Tuple，赋值。
                        Tuple<int, bool> likeObj = await likeObjTask;

                        blogDto.IsLike = likeObj.Item2;
                        blogDto.LikeCount = likeObj.Item1;

                        blogDtoList.Add(blogDto);
                    }
                }

                return blogDtoList;
            }
        }

        /// <summary>
        /// 获取用户自己的Blog
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public async Task<List<BlogDTO>> GetOwnBlogs(long personId, int pageIndex, int pageSize)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                List<BlogDTO> blogDtoList = null;

                BlogHandler blogHandler = new BlogHandler(dbContext);

                //1. 获取用户的Blogs集合。
                var blogs = await blogHandler.GetBlogsByPersonId(personId, pageIndex, pageSize);

                if(blogs.Count > 0)
                {
                    PersonHandler perHandler = new PersonHandler(dbContext);
                    CommentHandler commentHandler = new CommentHandler(dbContext);
                    BlogXBlogHandler bxbHandler = new BlogXBlogHandler(dbContext);
                    EntityLikeHandler likeHandler = new EntityLikeHandler(dbContext);
                    BlogXContentHandler bxcHandler = new BlogXContentHandler(dbContext);

                    blogDtoList = new List<BlogDTO>();

                    foreach (var blog in blogs)
                    {
                        BlogDTO blogDto = blog.ToDTO();

                        //2. 判断Person对象是否为空，如果为空则获取。
                        if (blogDto.Person == null)
                        {
                            var personEntity = await perHandler.GetByIdAsync(blog.PersonID);

                            if (personEntity != null)
                            {
                                blogDto.Person = personEntity.ToDTO();
                            }
                        }

                        //3. 判断Contents集合是否为空，如果为空则获取。
                        if (blogDto.Contents == null)
                        {
                            List<Content> contentList = await bxcHandler.GetContentsAsync(blogDto.ID);

                            if (contentList != null && contentList.Count > 0)
                            {
                                blogDto.Contents = new List<ContentDTO>();

                                foreach (var content in contentList)
                                {
                                    ContentDTO contentDto = content.ToDTO();

                                    blogDto.Contents.Add(contentDto);
                                }
                            }
                        }

                        //4. 判断此Blog是否转发了其他Blog。
                        if (blogDto.BaseBlog == null)
                        {
                            Blog baseBlog = await bxbHandler.GetBaseBlogByBlogIdAsync(blogDto.ID);

                            if (baseBlog != null)
                            {
                                blogDto.BaseBlog = baseBlog.ToDTO();

                                //4.1 判断转发的Blog的Person对象是否为空，如果为空则获取。不需要获取头像。
                                if (blogDto.BaseBlog.Person == null)
                                {
                                    var personEntity = await perHandler.GetByIdAsync(baseBlog.PersonID);

                                    if (personEntity != null)
                                    {
                                        blogDto.BaseBlog.Person = personEntity.ToDTO();
                                    }
                                }
                                //4.2 判断转发的Blog是否有发Contents。
                                if (blogDto.BaseBlog.Contents == null)
                                {
                                    List<Content> contentList = await bxcHandler.GetContentsAsync(blogDto.BaseBlog.ID);

                                    if (contentList != null && contentList.Count > 0)
                                    {
                                        blogDto.BaseBlog.Contents = new List<ContentDTO>();

                                        foreach (var content in contentList)
                                        {
                                            ContentDTO contentDto = content.ToDTO();

                                            blogDto.BaseBlog.Contents.Add(contentDto);
                                        }
                                    }
                                }

                                //4.3 获取转发的Blog的转发数量。
                                blogDto.BaseBlog.RepostCount = await bxbHandler.GetRepostCountAsync(blogDto.BaseBlog.ID);

                                //4.4 获取转发的Blog的评论数量。
                                blogDto.BaseBlog.CommentCount = await commentHandler.GetCommentCountAsync(blogDto.BaseBlog.ID);

                                //4.5 获取转发的Blog的点赞数量
                                blogDto.BaseBlog.LikeCount = await likeHandler.GetBlogLikeCountAsync(blogDto.BaseBlog.ID);

                                //4.6 获取转发的Blog是否已经点赞。
                                blogDto.BaseBlog.IsLike = await likeHandler.IsLikeAsync(personId, blogDto.BaseBlog.ID, typeof(Blog));
                            }
                        }

                        //5. 获取评论数量。
                        blogDto.CommentCount = await commentHandler.GetCommentCountAsync(blog.ID);

                        //6. 获取转发数量。
                        blogDto.RepostCount = await bxbHandler.GetRepostCountAsync(blog.ID);

                        //7. 获取点赞数量和用户是否已经点赞。
                        Tuple<int, bool> likeObj = await GetLikeObjectAsync(personId, blog.ID);

                        blogDto.IsLike = likeObj.Item2;
                        blogDto.LikeCount = likeObj.Item1;

                        blogDtoList.Add(blogDto);
                    }                    
                }

                return blogDtoList;
            }
        }

        /// <summary>
        /// Blog点赞或者取消赞
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<Tuple<bool, int>> LikeAsync(long personId, long blogId)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                EntityLikeHandler entityLikeHandler = new EntityLikeHandler(dbContext);

                return await entityLikeHandler.LikeAsync(personId, blogId, typeof(Blog));
            }
        }

        /// <summary>
        /// 收藏或者取消收藏
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<bool> CollectAsync(long personId, long blogId)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                FavoriteHandler favoriteHandler = new FavoriteHandler(dbContext);

                return await favoriteHandler.CollectAsync(personId, blogId);
            }
        }

        /// <summary>
        /// 获取Blog收藏的数量
        /// </summary>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<int> GetFavoriteCountAsync(long blogId)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                FavoriteHandler favoriteHandler = new FavoriteHandler(dbContext);

                return await favoriteHandler.GetFavoriteCountAsync(blogId);
            }
        }

        /// <summary>
        /// 获取Blog转发的数量
        /// </summary>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<int> GetRepostCountAsync(long blogId)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogXBlogHandler bxbHandler = new BlogXBlogHandler(dbContext);

                return await bxbHandler.GetRepostCountAsync(blogId);
            }
        }

        /// <summary>
        /// 获取Blog点赞的数量和用户是否点赞了Blog
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<Tuple<int, bool>> GetLikeObjectAsync(long personId, long blogId)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                EntityLikeHandler likeHandler = new EntityLikeHandler(dbContext);

                int likeCount = await likeHandler.GetBlogLikeCountAsync(blogId);

                bool isLike = await likeHandler.IsLikeAsync(personId, blogId, typeof(Blog));

                return new Tuple<int, bool>(likeCount, isLike);
            }
        }

        /// <summary>
        /// 获取Blog的Content集合。
        /// </summary>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<List<Content>> GetBlogContentsAsync(long blogId)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                BlogXContentHandler bxcHandler = new BlogXContentHandler(dbContext);

                return await bxcHandler.GetContentsAsync(blogId);
            }
        }

        /// <summary>
        /// 判断用户是否收藏了Blog
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<bool> IsFavoriteAsync(long personId, long blogId)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                FavoriteHandler favoriteHandler = new FavoriteHandler(dbContext);

                return await favoriteHandler.IsFavoriteAsync(personId, blogId);
            }
        }
    }
}
