using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class Avatar : EntityBase
    {
        public long ID { get; set; }
        public long PersonID { get; set; }
        public string AvatarPath { get; set; }
        public byte[] AvatarBinary { get; set; }
        public string MimeType { get; set; }
        public string AltAttribute { get; set; }
        public string TitleAttribute { get; set; }
        public string SeoFilename { get; set; }
        public bool IsActive { get; set; }
        public virtual Person Person { get; set; }
    }
}
