using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KoalaBlog.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "请输入用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "下次自动登录")]
        public bool RememberMe { get; set; }
    }

    public class ConfirmEmailViewModel
    {
        [Required]
        public long UserID { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public bool IsEmailConfirmed { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?:(?=.*[a-z])(?:(?=.*[A-Z])(?=.*[\\d\\W])|(?=.*\\W)(?=.*\\d))|(?=.*\\W)(?=.*[A-Z])(?=.*\\d)).{8,}$", ErrorMessage = "请输入8位以上数字、字母或常用符号,字母区分大小写")]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "确认密码")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "邮件地址")]
        public string Email { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码输入不匹配")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Code { get; set; }
    }
}