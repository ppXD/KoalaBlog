using FluentValidation;
using KoalaBlog.WebApi.Models;

namespace KoalaBlog.WebApi.Validators.Security
{
    public class IsUserInRoleBindingModelValidator : BaseValidator<IsUserInRoleBindingModel>
    {
        public IsUserInRoleBindingModelValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("UserName cannot be empty.");
            RuleFor(x => x.RolesOrPermissionsName)
                .NotNull()                
                .WithMessage("RolesOrPermissionsName cannot be null.");
        }
    }
}