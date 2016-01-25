using FluentValidation.Attributes;
using KoalaBlog.WebApi.Validators.Comment;
using System.Collections.Generic;

namespace KoalaBlog.WebApi.Models
{
    [Validator(typeof(AddCommentBindingModelValidator))]
    public class AddCommentBindingModel
    {
        public long PersonID { get; set; }
        public long BlogID { get; set; }
        public long? BaseCommentID { get; set; }
        public string Content { get; set; }
        public List<long> PhotoContentIds { get; set; }
    }
}