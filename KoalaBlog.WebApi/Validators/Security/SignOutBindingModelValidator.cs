using FluentValidation;
using KoalaBlog.WebApi.Models;
namespace KoalaBlog.WebApi.Validators.Security
{
    public class SignOutBindingModelValidator : BaseValidator<SignOutBindingModel>
    {
        public SignOutBindingModelValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("UserName cannot be empty.");
            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage("Token cannot be empty.");
        }
    }
}