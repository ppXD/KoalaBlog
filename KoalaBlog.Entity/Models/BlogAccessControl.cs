using KoalaBlog.Entity.Models.Enums;
using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class BlogAccessControl : EntityBase
    {
        public BlogAccessControl()
        {
            this.BlogAccessControlXGroups = new List<BlogAccessControlXGroup>();
        }

        public long ID { get; set; }
        public long BlogID { get; set; }
        public BlogInfoAccessInfo AccessLevel { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual ICollection<BlogAccessControlXGroup> BlogAccessControlXGroups { get; set; }
    }
}
