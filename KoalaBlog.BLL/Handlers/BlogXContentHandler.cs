using KoalaBlog.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class BlogXContentHandler : BlogXContentHandlerBase
    {
        private readonly DbContext _dbContext;

        public BlogXContentHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 获取Content集合
        /// </summary>
        /// <param name="blogId">BlogID</param>
        /// <returns></returns>
        public async Task<List<Content>> GetContentsAsync(long blogId)
        {
            List<Content> contentList = null;

            List<BlogXContent> bxcList = await Entities.Include(x => x.Content).Where(x => x.BlogID == blogId).ToListAsync();

            if(bxcList != null && bxcList.Count > 0)
            {
                contentList = new List<Content>();

                foreach (var bxc in bxcList)
                {
                    contentList.Add(bxc.Content);
                }
            }

            return contentList;
        }

        /// <summary>
        /// 根据PersonID获取Content集合。
        /// </summary>
        /// <param name="personId">PersonID</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, List<Content>>> GetContentsByPersonIdAsync(long personId, int pageIndex = 1, int pageSize = int.MaxValue)
        {
            BlogHandler blogHandler = new BlogHandler(_dbContext);

            bool isLoadedAll = true;

            List<Content> contentList = null;

            //1. 获取Blog集合。
            var blogs = await blogHandler.GetBlogsByPersonId(personId, pageIndex, pageSize);

            if(blogs.Count > 0)
            {
                isLoadedAll = false;

                //2. 获取Blog集合的ID集合。
                List<long> blogIds = blogs.Select(x => x.ID).ToList();

                //3. 获取包含BlogID集合的BlogXContent Entities。
                List<BlogXContent> bxcList = await Entities.Include(x => x.Content).Where(x => blogIds.Contains(x.BlogID)).ToListAsync();

                if (bxcList != null && bxcList.Count > 0)
                {
                    contentList = new List<Content>();

                    foreach (var bxc in bxcList)
                    {
                        contentList.Add(bxc.Content);
                    }
                }
            }

            return new Tuple<bool,List<Content>>(isLoadedAll, contentList);
        }
    }
}
