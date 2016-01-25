using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.DTOs
{
    public class BlogDTO
    {
        public long ID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string ContentText { get; set; }
        /// <summary>
        /// 是否已经点赞
        /// </summary>
        public bool IsLike { get; set; }
        /// <summary>
        ///  是否已经收藏 
        /// </summary>
        public bool IsFavorite { get; set; }
        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentCount { get; set; }
        /// <summary>
        /// 转发数量
        /// </summary>
        public int RepostCount { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>
        public int LikeCount { get; set; }

        public BlogDTO BaseBlog { get; set; }
        public PersonDTO Person { get; set; }
        public List<ContentDTO> Contents { get; set; }
    }
}
