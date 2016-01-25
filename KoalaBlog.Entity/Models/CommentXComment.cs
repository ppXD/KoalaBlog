using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class CommentXComment : EntityBase
    {
        public long ID { get; set; }
        public long BaseCommentID { get; set; }
        public long NewCommentID { get; set; }
        public virtual Comment BaseComment { get; set; }
        public virtual Comment NewComment { get; set; }
    }
}
