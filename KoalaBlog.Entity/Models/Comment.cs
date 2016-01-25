using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class Comment : EntityBase
    {
        public Comment()
        {
            this.BaseCommentXComments = new List<CommentXComment>();
            this.NewCommentXComments = new List<CommentXComment>();
            this.CommentXContents = new List<CommentXContent>();
        }

        public long ID { get; set; }
        public long PersonID { get; set; }
        public long BlogID { get; set; }
        public string Content { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<CommentXComment> BaseCommentXComments { get; set; }
        public virtual ICollection<CommentXComment> NewCommentXComments { get; set; }
        public virtual ICollection<CommentXContent> CommentXContents { get; set; }
    }
}
