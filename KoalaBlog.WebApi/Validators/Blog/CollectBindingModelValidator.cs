using FluentValidation;
using KoalaBlog.WebApi.Models;
namespace KoalaBlog.WebApi.Validators.Blog
{
    public class CollectBindingModelValidator : BaseValidator<CollectBindingModel>
    {
        public CollectBindingModelValidator()
        {
            RuleFor(x => x.PersonID)
                .GreaterThan(0)
                .WithMessage("Invalid");
            RuleFor(x => x.BlogID)
                .GreaterThan(0)
                .WithMessage("Invalid");
        }
    }
}