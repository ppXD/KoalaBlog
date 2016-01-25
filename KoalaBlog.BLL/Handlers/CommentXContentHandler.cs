using KoalaBlog.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class CommentXContentHandler : CommentXContentHandlerBase
    {
        private readonly DbContext _dbContext;

        public CommentXContentHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 获取Content集合
        /// </summary>
        /// <param name="commentId">CommentID</param>
        /// <returns></returns>
        public async Task<List<Content>> GetContentsAsync(long commentId)
        {
            List<Content> contentList = null;

            List<CommentXContent> cxcList = await Entities.Include(x => x.Content).Where(x => x.CommentID == commentId).ToListAsync();

            if (cxcList != null && cxcList.Count > 0)
            {
                contentList = new List<Content>();

                foreach (var cxc in cxcList)
                {
                    contentList.Add(cxc.Content);
                }
            }

            return contentList;
        }
    }
}
