using FluentValidation;
using FluentValidation.Attributes;
using KoalaBlog.WebApi.Validators.Security;

namespace KoalaBlog.WebApi.Models
{
    [Validator(typeof(SignInBindingModelValidator))]
    public class SignInBindingModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
    }

    [Validator(typeof(SignOutBindingModelValidator))]
    public class SignOutBindingModel
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }

    [Validator(typeof(RegisterBindingModelValidator))]
    public class RegisterBindingModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    [Validator(typeof(IsUserInRoleBindingModelValidator))]
    public class IsUserInRoleBindingModel
    {
        public string UserName { get; set; }
        public string[] RolesOrPermissionsName { get; set; }
    }

    [Validator(typeof(ResetPasswordBindingModelValidator))]
    public class ResetPasswordBindingModel
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}