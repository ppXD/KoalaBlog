using KoalaBlog.Entity.Models;
using KoalaBlog.Entity.Models.Enums;
using KoalaBlog.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class CommentHandler : CommentHandlerBase
    {
        private readonly DbContext _dbContext;

        public CommentHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 添加一条评论
        /// </summary>
        /// <param name="personId">评论者ID</param>
        /// <param name="blogId">评论的BlogID</param>
        /// <param name="content">评论内容</param>
        /// <param name="photoContentIds">评论的附件ID(仅限图片)</param>
        /// <param name="baseCommentId">被评论的CommentID</param>
        /// <returns></returns>
        public async Task<Comment> AddCommentAsync(long personId, long blogId, string content, List<long> photoContentIds = null, long? baseCommentId = null)
        {
            BlogHandler blogHandler = new BlogHandler(_dbContext);        
            GroupHandler groupHandler = new GroupHandler(_dbContext);
            PersonHandler perHandler = new PersonHandler(_dbContext);

            //1. 检查要评论的Blog是否存在。
            Blog beCommentBlog = await blogHandler.GetByIdAsync(blogId);

            //2. 如果为空或者被逻辑删除，则Exception。
            if (beCommentBlog == null || beCommentBlog.IsDeleted)
            {
                throw new DisplayableException("要评论的Blog不存在或者已经被删除");
            }

            //2.1 自己评论自己的Blog永远可以，所以只需要判断不同的PersonID。
            if(beCommentBlog.PersonID != personId)
            {
                //3. 检查当前用户是否被该评论Blog的用户加入了黑名单，如果是则不能进行评论。
                Group beCommentBlogPersonBlackList = await groupHandler.Include(x => x.GroupMembers).SingleOrDefaultAsync(x => x.PersonID == beCommentBlog.PersonID && x.Type == GroupType.BlackList);

                if (beCommentBlogPersonBlackList != null && beCommentBlogPersonBlackList.GroupMembers.Count > 0)
                {
                    bool isBlocked = beCommentBlogPersonBlackList.GroupMembers.Select(x => x.PersonID).Contains(personId);

                    if (isBlocked)
                    {
                        throw new DisplayableException("由于用户设置，你无法回复评论。");
                    }
                }

                //4. 检查评论Blog的用户消息设置，是否允许评论。
                Person beCommentBlogPerson = await perHandler.GetByIdAsync(beCommentBlog.PersonID);

                if (beCommentBlogPerson != null)
                {
                    //4.1 如果评论只允许关注的人评论，则判断Blog的用户的是否关注了当前用户。
                    if (beCommentBlogPerson.AllowablePersonForComment == AllowablePersonForComment.FollowerOnly)
                    {
                        //4.2 判断关注的人集合是否已经加载。
                        if (!_dbContext.Entry(beCommentBlogPerson).Collection(x => x.MyFollowingPersons).IsLoaded)
                        {
                            _dbContext.Entry(beCommentBlogPerson).Collection(x => x.MyFollowingPersons).Load();
                        }

                        bool isFollow = beCommentBlogPerson.MyFollowingPersons.Select(x => x.FollowingID).Contains(personId);

                        if (!isFollow)
                        {
                            throw new DisplayableException("由于用户设置，你无法回复评论。");
                        }
                    }
                    //4.3 如果评论只允许粉丝评论，则判断当前用户是否关注了该Blog用户。
                    else if (beCommentBlogPerson.AllowablePersonForComment == AllowablePersonForComment.FansOnly)
                    {
                        //4.4 判断粉丝集合是否已经加载。
                        if (!_dbContext.Entry(beCommentBlogPerson).Collection(x => x.MyFans).IsLoaded)
                        {
                            _dbContext.Entry(beCommentBlogPerson).Collection(x => x.MyFans).Load();
                        }

                        bool isFans = beCommentBlogPerson.MyFans.Select(x => x.FollowerID).Contains(personId);

                        if (!isFans)
                        {
                            throw new DisplayableException("由于用户设置，你无法回复评论。");
                        }
                    }
                }

                //5. 检查评论Blog的用户消息设置，是否允许评论附带图片。
                if(photoContentIds != null && photoContentIds.Count > 0)
                {
                    if(!beCommentBlogPerson.AllowCommentAttachContent)
                    {
                        throw new DisplayableException("由于用户设置，你回复评论无法添加图片。");
                    }
                }
            }
            
            using(var dbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    //6. 建立Comment对象并保存。
                    Comment comment = new Comment()
                    {
                        PersonID = personId,
                        BlogID = blogId,
                        Content = content
                    };
                    this.Add(comment);
                    await SaveChangesAsync();

                    //7. 判断是否有图片，有则建立与Comment的关联。
                    if (photoContentIds != null && photoContentIds.Count > 0)
                    {
                        if(photoContentIds.Count > 6)
                        {
                            throw new DisplayableException("评论最多附件6张图片");
                        }

                        ContentHandler contentHandler = new ContentHandler(_dbContext);

                        //7.1 判断附件是否为图片。
                        List<Content> photoContents = await contentHandler.Fetch(x => photoContentIds.Contains(x.ID)).ToListAsync();

                        if(!photoContents.Any(x => x.Type == ContentType.Photo))
                        {
                            throw new DisplayableException("评论只能附件图片");
                        }

                        //7.2 建立Comment与Content的关联。
                        CommentXContentHandler cxcHandler = new CommentXContentHandler(_dbContext);

                        foreach (var photoContentId in photoContentIds)
                        {
                            CommentXContent cxc = new CommentXContent()
                            {
                                CommentID = comment.ID,
                                ContentID = photoContentId
                            };
                            cxcHandler.Add(cxc);
                        }
                        await SaveChangesAsync();
                    }

                    //8. 判断当前评论是否评论Blog里的其他评论，如果是则建立关联。
                    if(baseCommentId != null)
                    {
                        //8.1 判断被评论ID是否存在。
                        Comment baseComment = await GetByIdAsync(baseCommentId);

                        if(baseComment == null)
                        {
                            throw new DisplayableException("此评论不存在或者已经被删除");
                        }

                        //8.2 判断被评论的BlogID是否与当前评论的BlogID一致。
                        if(baseComment.BlogID != blogId)
                        {
                            throw new DisplayableException("此评论的BlogID与被评论的BlogID不一致");
                        }

                        //8.3 建立关联。
                        CommentXCommentHandler cxcHandler = new CommentXCommentHandler(_dbContext);

                        CommentXComment cxc = new CommentXComment()
                        {
                            BaseCommentID = (long)baseCommentId,
                            NewCommentID = comment.ID
                        };
                        cxcHandler.Add(cxc);
                        await SaveChangesAsync();
                    }

                    dbTransaction.Commit();

                    return comment;
                }
                catch (Exception)
                {
                    dbTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 获取Blog的评论
        /// </summary>
        /// <param name="blogId">BlogID</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<Comment>> GetCommentsAsync(long blogId, int pageIndex = 1, int pageSize = int.MaxValue)
        {
            BlogHandler blogHandler = new BlogHandler(_dbContext);

            //1. 判断获取Comment的Blog是否存在或者被删除。
            Blog getCommentsBlog = await blogHandler.GetByIdAsync(blogId);

            //2. 如果为空或者被逻辑删除，则Exception。
            if (getCommentsBlog == null || getCommentsBlog.IsDeleted)
            {
                throw new DisplayableException("Blog不存在或者已经被删除");
            }

            //3. 获取评论。
            List<Comment> comments = await Fetch(x => x.BlogID == blogId).OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return comments;
        }

        /// <summary>
        /// 获取Blog评论的数量
        /// </summary>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<int> GetCommentCountAsync(long blogId)
        {
            return await CountAsync(x => x.BlogID == blogId);
        }
    }
}
