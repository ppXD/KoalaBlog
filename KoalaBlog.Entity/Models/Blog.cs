using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class Blog : EntityBase
    {
        public Blog()
        {
            this.BlogAccessControls = new List<BlogAccessControl>();
            this.BaseBlogXBlogs = new List<BlogXBlog>();
            this.NewBlogXBlogs = new List<BlogXBlog>();
            this.BlogXContents = new List<BlogXContent>();
            this.Comments = new List<Comment>();
            this.Favorites = new List<Favorite>();
        }

        public long ID { get; set; }
        public long PersonID { get; set; }
        public string Content { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<BlogAccessControl> BlogAccessControls { get; set; }
        public virtual ICollection<BlogXBlog> BaseBlogXBlogs { get; set; }
        public virtual ICollection<BlogXBlog> NewBlogXBlogs { get; set; }
        public virtual ICollection<BlogXContent> BlogXContents { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
    }
}
