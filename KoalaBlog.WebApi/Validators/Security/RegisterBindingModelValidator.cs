using FluentValidation;
using KoalaBlog.WebApi.Models;

namespace KoalaBlog.WebApi.Validators.Security
{
    public class RegisterBindingModelValidator : BaseValidator<RegisterBindingModel>
    {
        public RegisterBindingModelValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("UserName cannot be empty.");
            RuleFor(x => x.Password)
                .NotEmpty()
                .Length(8, 20)
                .WithMessage("Invalid");
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Invalid");
        }
    }
}