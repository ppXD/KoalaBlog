using FluentValidation;
using KoalaBlog.WebApi.Models;

namespace KoalaBlog.WebApi.Validators.Security
{
    public class SignInBindingModelValidator : BaseValidator<SignInBindingModel>
    {
        public SignInBindingModelValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("UserName cannot be empty.");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password cannot be empty.");
        }
    }
}