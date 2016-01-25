using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class BlogAccessControlXGroup : EntityBase
    {
        public long ID { get; set; }
        public long BlogAccessControlID { get; set; }
        public long GroupID { get; set; }
        public virtual BlogAccessControl BlogAccessControl { get; set; }
        public virtual Group Group { get; set; }
    }
}
