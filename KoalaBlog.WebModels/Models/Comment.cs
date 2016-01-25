using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.WebModels.Models
{
    public class Comment
    {
        public long ID { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ContentText { get; set; }

        public bool IsLike { get; set; }

        public int LikeCount { get; set; }

        public Person Person { get; set; }

        public Comment BaseComment { get; set; }

        public List<Content> Contents { get; set; }
    }
}
