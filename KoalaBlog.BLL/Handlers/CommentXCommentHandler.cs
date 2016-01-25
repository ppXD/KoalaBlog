using KoalaBlog.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class CommentXCommentHandler : CommentXCommentHandlerBase
    {
        private readonly DbContext _dbContext;

        public CommentXCommentHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 根据CommentID获取BaseComment对象
        /// </summary>
        /// <param name="commentId">CommentID</param>
        /// <returns></returns>
        public async Task<Comment> GetBaseCommentByCommentIdAsync(long commentId)
        {
            Comment baseComment = null;

            CommentXComment cxc = await Entities.Include(x => x.BaseComment).Where(x => x.NewCommentID == commentId).SingleOrDefaultAsync();

            if(cxc != null)
            {
                baseComment = cxc.BaseComment;
            }

            return baseComment;
        }
    }
}
