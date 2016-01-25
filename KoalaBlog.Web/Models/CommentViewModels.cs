using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KoalaBlog.Web.Models
{
    public class AddCommentViewModel
    {
        public long BlogID { get; set; }
        public long? BaseCommentID { get; set; }
        [Required]
        public string Content { get; set; }
        public List<long> photoContentIds { get; set; }
    }
}