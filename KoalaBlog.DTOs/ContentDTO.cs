using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.DTOs
{
    public class ContentDTO
    {
        public long ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ContentPath { get; set; }
        public byte[] ContentBinary { get; set; }
        public string MimeType { get; set; }
        public string AltAttribute { get; set; }
        public string TitleAttribute { get; set; }
        public string SeoFilename { get; set; }
        public string Type { get; set; }
    }
}
