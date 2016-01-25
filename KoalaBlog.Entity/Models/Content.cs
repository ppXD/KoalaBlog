using KoalaBlog.Entity.Models.Enums;
using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class Content : EntityBase
    {
        public Content()
        {
            this.BlogXContents = new List<BlogXContent>();
            this.CommentXContents = new List<CommentXContent>();
        }

        public long ID { get; set; }
        public string ContentPath { get; set; }
        public byte[] ContentBinary { get; set; }
        public string MimeType { get; set; }
        public string AltAttribute { get; set; }
        public string TitleAttribute { get; set; }
        public string SeoFilename { get; set; }
        public ContentType Type { get; set; }
        public virtual ICollection<BlogXContent> BlogXContents { get; set; }
        public virtual ICollection<CommentXContent> CommentXContents { get; set; }
    }
}
