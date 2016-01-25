using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KoalaBlog.Web.Models
{
    public class CreateBlogViewModel
    {
        public long? GroupID { get; set; }
        public long? ForwardBlogID { get; set; }
        [Required]
        public string Content { get; set; }
        public int AccessInfo { get; set; }
        public List<long> AttachContentIds { get; set; }
    }
}