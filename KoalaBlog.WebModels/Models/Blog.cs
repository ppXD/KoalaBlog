using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.WebModels
{
    public class Blog
    {
        public long ID { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ContentText { get; set; }

        public bool IsLike { get; set; }

        public bool IsFavorite { get; set; }

        public int CommentCount { get; set; }

        public int RepostCount { get; set; }

        public int LikeCount { get; set; }

        public Blog BaseBlog { get; set; }

        public Person Person { get; set; }

        public List<Content> Contents { get; set; }
    }
}
