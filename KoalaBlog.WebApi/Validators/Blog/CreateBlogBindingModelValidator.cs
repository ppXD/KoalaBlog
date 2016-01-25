using FluentValidation;
using KoalaBlog.WebApi.Models;

namespace KoalaBlog.WebApi.Validators.Blog
{
    public class CreateBlogBindingModelValidator : BaseValidator<CreateBlogBindingModel>
    {
        public CreateBlogBindingModelValidator()
        {
            RuleFor(x => x.PersonID)
                .GreaterThan(0)
                .WithMessage("Invalid");
            RuleFor(x => x.GroupID)
                .GreaterThan(0)
                .When(x => x.GroupID != null)
                .WithMessage("Invalid");
            RuleFor(x => x.ForwardBlogID)
                .GreaterThan(0)
                .When(x => x.ForwardBlogID != null)
                .WithMessage("Invalid");
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content cannot be empty.");
            RuleFor(x => x.AccessInfo)
                .NotNull()
                .WithMessage("AccessInfo cannot be null.");
        }
    }
}