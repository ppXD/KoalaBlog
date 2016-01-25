using KoalaBlog.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class BlogXBlogHandler : BlogXBlogHandlerBase
    {
        private readonly DbContext _dbContext;

        public BlogXBlogHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 根据BlogID获取BaseBlog对象
        /// </summary>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<Blog> GetBaseBlogByBlogIdAsync(long blogId)
        {
            Blog baseBlog = null;

            BlogXBlog bxb = await Entities.Include(x => x.BaseBlog).Where(x => x.NewBlogID == blogId && x.IsBase).SingleOrDefaultAsync();

            if (bxb != null)
            {
                baseBlog = bxb.BaseBlog;
            }

            return baseBlog;
        }

        /// <summary>
        /// 获取Blog转发的数量
        /// </summary>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<int> GetRepostCountAsync(long blogId)
        {
            return await CountAsync(x => x.BaseBlogID == blogId);
        }
    }
}
