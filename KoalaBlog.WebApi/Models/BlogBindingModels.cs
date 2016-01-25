using FluentValidation.Attributes;
using KoalaBlog.Entity.Models.Enums;
using KoalaBlog.WebApi.Validators.Blog;
using System.Collections.Generic;

namespace KoalaBlog.WebApi.Models
{
    [Validator(typeof(CreateBlogBindingModelValidator))]
    public class CreateBlogBindingModel
    {
        public long PersonID { get; set; }
        public long? GroupID { get; set; }
        public long? ForwardBlogID { get; set; }
        public string Content { get; set; }
        public BlogInfoAccessInfo AccessInfo { get; set; }
        public List<long> AttachContentIds { get; set; }
    }

    [Validator(typeof(CollectBindingModelValidator))]
    public class CollectBindingModel
    {
        public long PersonID { get; set; }
        public long BlogID { get; set; }
    }
}