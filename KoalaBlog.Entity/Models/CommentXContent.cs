using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class CommentXContent : EntityBase
    {
        public long ID { get; set; }
        public long CommentID { get; set; }
        public long ContentID { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual Content Content { get; set; }
    }
}
