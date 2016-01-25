using FluentValidation;
using KoalaBlog.WebApi.Models;

namespace KoalaBlog.WebApi.Validators.Comment
{
    public class AddCommentBindingModelValidator : BaseValidator<AddCommentBindingModel>
    {
        public AddCommentBindingModelValidator()
        {
            RuleFor(x => x.PersonID)
                .GreaterThan(0)
                .WithMessage("Invalid");
            RuleFor(x => x.BlogID)
                .GreaterThan(0)
                .WithMessage("Invalid");
            RuleFor(x => x.BaseCommentID)
                .GreaterThan(0)
                .When(x => x.BaseCommentID != null)
                .WithMessage("Invalid");
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content cannot be empty.");
        }
    }
}