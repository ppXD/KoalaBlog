using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class BlogXContent : EntityBase
    {
        public long ID { get; set; }
        public long BlogID { get; set; }
        public long ContentID { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual Content Content { get; set; }
    }
}
