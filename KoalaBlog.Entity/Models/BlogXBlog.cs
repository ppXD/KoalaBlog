using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Entity.Models
{
    public partial class BlogXBlog : EntityBase
    {
        public long ID { get; set; }
        public long BaseBlogID { get; set; }
        public long NewBlogID { get; set; }
        public bool IsBase { get; set; }
        public virtual Blog BaseBlog { get; set; }
        public virtual Blog NewBlog { get; set; }
    }
}
