using FluentValidation;
using KoalaBlog.WebApi.Models;

namespace KoalaBlog.WebApi.Validators.Security
{
    public class ResetPasswordBindingModelValidator : BaseValidator<ResetPasswordBindingModel>
    {
        public ResetPasswordBindingModelValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Invalid");
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .Length(8, 20)
                .WithMessage("Invalid");
        }
    }
}