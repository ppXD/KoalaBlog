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
    public class BlogHandler : BlogHandlerBase
    {
        private readonly DbContext _dbContext;

        public BlogHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 发表一篇Blog
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="content">Blog的内容</param>
        /// <param name="attachContents">Blog的附件</param>
        /// <param name="accessInfo">Blog的访问控制</param>
        /// <param name="groupId">当Blog的访问控制为群可见则需要指定GroupID</param>
        /// <param name="forwardBlogId">转发的BlogID</param>
        /// <returns></returns>
        public async Task<Blog> CreateBlogAsync(long personId, string content, BlogInfoAccessInfo accessInfo, long? groupId = null, List<long> attachContentIds = null, long? forwardBlogId = null)
        {
            using(var dbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
	            {
                    BlogAccessControlHandler bacHandler = new BlogAccessControlHandler(_dbContext);

		            //1. 创建Blog对象并保存。
                    Blog blog = new Blog()
                    {
                        PersonID = personId,
                        Content = content,
                        IsDeleted = false
                    };
                    this.Add(blog);
                    await SaveChangesAsync();

                    //2. 判断是否有附件，有则建立与Blog的关联。
                    if (attachContentIds != null && attachContentIds.Count > 0)
                    {
                        ContentHandler contentHandler = new ContentHandler(_dbContext);
                        BlogXContentHandler bxcHandler = new BlogXContentHandler(_dbContext);

                        #region 检查attachContent集合。

                        List<Content> attachContents = await contentHandler.Fetch(x => attachContentIds.Contains(x.ID)).ToListAsync();

                        bool isDifferentType = attachContents.Select(x => x.Type).Distinct().Count() > 1;

                        if(isDifferentType)
                        {
                            throw new DisplayableException("不能发不同类型内容的Blog");
                        }

                        int photoCount = attachContents.Count(x => x.Type == ContentType.Photo);
                        int musicCount = attachContents.Count(x => x.Type == ContentType.Music);
                        int videoCount = attachContents.Count(x => x.Type == ContentType.Video);

                        if(photoCount > 6)
                        {
                            throw new DisplayableException("发表图片不能超过6张");
                        }
                        if (musicCount > 1)
                        {
                            throw new DisplayableException("发表音乐不能超过1首");
                        }
                        if (videoCount > 1)
                        {
                            throw new DisplayableException("发表视频不能超过1个");
                        }
                        #endregion

                        foreach (var attachContentId in attachContentIds)
                        {
                            BlogXContent bxc = new BlogXContent()
                            {
                                BlogID = blog.ID,
                                ContentID = attachContentId
                            };
                            bxcHandler.Add(bxc);
                        }
                        
                        await SaveChangesAsync();
                    }

                    //3. 判断当前是否转发Blog，是则建立转发Blog与新Blog的关联。
                    if(forwardBlogId.HasValue)
                    {
                        //3.1 判断ForwardBlog是否存在。
                        Blog forwardBlog = await GetByIdAsync(forwardBlogId);

                        if(forwardBlog == null || forwardBlog.IsDeleted)
                        {
                            throw new DisplayableException("该Blog不存在或者已经被删除");
                        }

                        //3.2 判断ForwardBlog是否转发了Blog，如果是则关联2条Blog，否则只关联当前要转发的Blog。
                        BlogXBlogHandler bxbHandler = new BlogXBlogHandler(_dbContext);

                        BlogXBlog blogXblog = new BlogXBlog();
                        blogXblog.NewBlogID = blog.ID;
                        blogXblog.BaseBlogID = forwardBlog.ID;
                        blogXblog.IsBase = false;

                        //3.3 判断ForwardBlog是否有转发Blog。
                        Blog baseBlog = await bxbHandler.GetBaseBlogByBlogIdAsync(forwardBlog.ID);

                        if(baseBlog == null)
                        {
                            blogXblog.IsBase = true;
                        }
                        else
                        {
                            BlogXBlog baseBlogXblog = new BlogXBlog();
                            baseBlogXblog.NewBlogID = blog.ID;
                            baseBlogXblog.BaseBlogID = baseBlog.ID;
                            baseBlogXblog.IsBase = true;

                            bxbHandler.Add(baseBlogXblog);
                        }

                        bxbHandler.Add(blogXblog);

                        await SaveChangesAsync();
                    }

                    //4. 建立AccessControl访问控制。
                    BlogAccessControl blogAccessControl = new BlogAccessControl()
                    {
                        BlogID = blog.ID,
                        AccessLevel = accessInfo
                    };
                    bacHandler.Add(blogAccessControl);
                    await SaveChangesAsync();

                    //5. 判断如果访问控制为GroupOnly，则建立AccessControlXGroup关系。
                    if(accessInfo == BlogInfoAccessInfo.GroupOnly)
                    {
                        if(groupId == null)
                        {
                            throw new DisplayableException("未指定组");
                        }

                        BlogAccessControlXGroupHandler bacxgHandler = new BlogAccessControlXGroupHandler(_dbContext);

                        BlogAccessControlXGroup bacxg = new BlogAccessControlXGroup()
                        {
                            BlogAccessControlID = blogAccessControl.ID,
                            GroupID = (long)groupId
                        };
                        bacxgHandler.Add(bacxg);
                        await SaveChangesAsync();
                    }

                    dbTransaction.Commit();

                    return blog;
	            }
	            catch (Exception)
	            {
		            dbTransaction.Rollback();
		            throw;
	            }
            }
        }

        /// <summary>
        /// 获取当前用户关注的人Blog
        /// </summary>
        /// <param name="personId">用户ID</param>
        /// <param name="groupId">组ID</param>
        /// <returns></returns>
        public async Task<List<Blog>> GetBlogsAsync(long personId, long? groupId = null, int pageIndex = 1, int pageSize = int.MaxValue)
        {
            PersonHandler perHandler = new PersonHandler(_dbContext);
            GroupHandler groupHandler = new GroupHandler(_dbContext);
            BlogAccessControlHandler acHandler = new BlogAccessControlHandler(_dbContext);

            //1. 获取当前用户Person实体，并加载正在关注列表。
            Person per = await perHandler.Include(x => x.MyFollowingPersons).SingleOrDefaultAsync(x => x.ID == personId);

            if(per == null)
            {
                throw new DisplayableException("该用户不存在");
            }

            //2. 获取组成员或者已关注的用户ID集合，若指定了GroupID，则获取组里成员ID集合，反则获取已关注的人ID集合。
            List<long> perIds = new List<long>();

            if(groupId.HasValue)
            {
                Group normalGroup = await groupHandler.Include(x => x.GroupMembers).SingleOrDefaultAsync(x => x.ID == groupId);

                if(normalGroup != null)
                {
                    if(normalGroup.PersonID != personId)
                    {
                        throw new DisplayableException("该组不属于此用户");
                    }

                    //2.1 获取指定Group的用户ID集合。
                    if(normalGroup.GroupMembers.Count > 0)
                    {
                        perIds = normalGroup.GroupMembers.Select(x => x.PersonID).ToList();
                    }
                }
            }
            else
            {
                //2.2 获取当前用户已关注的用户ID集合。
                perIds = per.MyFollowingPersons.Select(x => x.FollowingID).ToList();
            }

            //2.3 加上当前用户的ID，提供查询当前用户的Blog。
            perIds.Add(personId);

            //3. 获取当前用户屏蔽名单Group。
            Group shieldGroup = await groupHandler.Include(x => x.GroupMembers).SingleOrDefaultAsync(x => x.PersonID == personId && x.Type == GroupType.ShieldList);

            if (shieldGroup != null && shieldGroup.GroupMembers.Count > 0)
            {
                List<long> shieldListIds = shieldGroup.GroupMembers.Select(x => x.PersonID).ToList();

                //3.1 过滤屏蔽名单上的用户，不加载屏蔽名单上用户的Blog。
                perIds = perIds.Except(shieldListIds).ToList();
            }

            //4. 获取过滤后的用户ID集合的Blog列表，以创建时间降序排序。
            List<Blog> blogs = await Fetch(x => perIds.Contains(x.PersonID) && !x.IsDeleted).OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            if(blogs.Count > 0)
            {
                //5.1 获取排除当前用户ID的BlogIDs集合。
                List<long> blogListIds = blogs.Where(x => x.PersonID != personId).Select(x => x.ID).ToList();

                //5.2 获取Blog列表的AccessControl(权限控制)列表，排除公开的权限控制。
                List<BlogAccessControl> blogAccessControls = await acHandler.Fetch(x => blogListIds.Contains(x.BlogID) && x.AccessLevel != BlogInfoAccessInfo.All).ToListAsync();

                foreach (var blogAccessControl in blogAccessControls)
                {
                    Blog acBlog = blogs.Single(x => x.ID == blogAccessControl.BlogID);

                    //5.3 如果权限控制为仅自己可见，且发表Blog的用户ID不等于当前用户ID，则过滤掉此Blog。
                    if (blogAccessControl.AccessLevel == BlogInfoAccessInfo.MyselfOnly)
                    {
                        blogs.Remove(acBlog);
                    }
                    //5.4 如果权限控制为群可见，判断此群的人员名单是否包含当前用户，没有则过滤此Blog。
                    else if (blogAccessControl.AccessLevel == BlogInfoAccessInfo.GroupOnly)
                    {
                        BlogAccessControlXGroupHandler acxgHandler = new BlogAccessControlXGroupHandler(_dbContext);

                        BlogAccessControlXGroup acxGroup = await acxgHandler.Include(x => x.Group, x => x.Group.GroupMembers).SingleOrDefaultAsync(x => x.BlogAccessControlID == blogAccessControl.ID);

                        if (acxGroup != null)
                        {
                            bool isInGroupMember = false;

                            if (acxGroup.Group != null && acxGroup.Group.GroupMembers.Count > 0)
                            {
                                isInGroupMember = acxGroup.Group.GroupMembers.Select(x => x.PersonID).Any(x => x == personId);
                            }

                            if (!isInGroupMember)
                            {
                                blogs.Remove(acBlog);
                            }
                        }
                    }
                    //5.5 如果权限控制为朋友圈可见，判断当前用户是否与互相关注(Friends)，如果不是则过滤此Blog。
                    else if (blogAccessControl.AccessLevel == BlogInfoAccessInfo.FriendOnly)
                    {
                        PersonXPersonHandler pxpHandler = new PersonXPersonHandler(_dbContext);

                        bool isFriend = await pxpHandler.IsFriendAsync(personId, acBlog.PersonID);

                        if (!isFriend)
                        {
                            blogs.Remove(acBlog);
                        }
                    }
                }
            }
            return blogs;
        }

        /// <summary>
        /// 根据PersonID获取Blogs
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public async Task<List<Blog>> GetBlogsByPersonId(long personId, int pageIndex = 1, int pageSize = int.MaxValue)
        {
            List<Blog> blogs = await Fetch(x => !x.IsDeleted && x.PersonID == personId).OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return blogs;
        }

        /// <summary>
        /// 获取发表Blog的数量
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <returns></returns>
        public async Task<int> GetBlogCountAsync(long personId)
        {
            return await CountAsync(x => x.PersonID == personId && !x.IsDeleted);
        }
    }
}
