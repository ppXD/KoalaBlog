using KoalaBlog.Entity.Models;
using KoalaBlog.Framework.Common;
using KoalaBlog.Framework.Enums;
using KoalaBlog.Principal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using KoalaBlog.Framework.MVC;
using KoalaBlog.Web.Models;
using KoalaBlog.Framework.Net;
using KoalaBlog.Framework.Security;
using KoalaBlog.Entity.Models.Enums;
using KoalaBlog.Security;
using KoalaBlog.ApiClient;

namespace KoalaBlog.Web.Controllers
{
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                Tuple<KoalaBlogIdentityObject, SignInStatus, string> result = await Services.SecurityClient.SignInAsync(model.UserName, model.Password, model.RememberMe);

                var identityObj = result.Item1;
                var signInStatus = result.Item2;
                var accessToken = result.Item3;

                switch (signInStatus)
                {
                    case SignInStatus.Succeeded:
                        KoalaBlogSecurityManager.SetAuthCookie(accessToken);
                        KoalaBlogIdentity identity = new KoalaBlogIdentity(identityObj);
                        KoalaBlogPrincipal principal = new KoalaBlogPrincipal(identity);
                        System.Threading.Thread.CurrentPrincipal = principal;
                        return RedirectToLocal(returnUrl);

                    case SignInStatus.NotYetEmailConfirmed:
                        ConfirmEmailViewModel cevModel = new ConfirmEmailViewModel() { UserID = identityObj.UserID, Email = identityObj.Email, IsEmailConfirmed = false };
                        return View("ConfirmEmail", cevModel);

                    case SignInStatus.LockedOut:
                        return View("LockedOut");

                    case SignInStatus.Failure:
                        AddErrors("账号密码错误");
                        break;
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> LogOff()
        {
            SignOutStatus result = await Services.SecurityClient.SignOutAsync(CurrentThreadIdentityObject.UserName, KoalaBlogSecurityManager.GetAuthCookie());

            if(result == SignOutStatus.Succeeded)
            {
                KoalaBlogSecurityManager.RemoveAuthCookie();
            }

            return Redirect("/");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                Tuple<object, RegisterStatus> result = await Services.SecurityClient.RegisterAsync(model.UserName, model.Password, model.Email);

                var registerUser = result.Item1;
                var registerStatus = result.Item2;

                switch (registerStatus)
                {
                    case RegisterStatus.Succeeded:

                        await SendEmailConfirmationTokenAsync(0, "", "Confirm your account.", EmailConfirmationType.Register);

                        return View();

                    case RegisterStatus.Failure:
                        AddErrors("注册失败");
                        return View();
                }
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string email, string code)
        {
            if(email == null || code == null)
            {
                ViewBag.ErrorMsg = "验证邮件失败，请重新验证";
                return View("Error");
            }

            bool isEmailConfirmed = await Services.SecurityClient.ConfirmEmailAsync(email, code);

            ConfirmEmailViewModel cevModel = new ConfirmEmailViewModel() { Email = email, IsEmailConfirmed = isEmailConfirmed };

            return View(cevModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResendRegisterEmailConfirmation(ConfirmEmailViewModel model)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.ErrorMsg = "重发邮件失败";
                return View("Error");
            }
            else
            {
                await SendEmailConfirmationTokenAsync(model.UserID, model.Email, "Confirm your account.", EmailConfirmationType.Register);
                return View("ConfirmEmail", model);
            }
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await Services.SecurityClient.GetSafeUserAccountByEmailAsync(model.Email);                

                if(user == null)
                {
                    AddErrors("该用户不存在");
                    return View(model);
                }
                if(!(user as UserAccount).EmailConfirmed)
                {
                    AddErrors("该用户尚未激活");
                    return View(model);
                }

                await SendEmailConfirmationTokenAsync((user as UserAccount).ID, model.Email, "验证并重置密码.", EmailConfirmationType.ResetPassword);

                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string email, string code)
        {
            if (email == null || code == null)
            {
                ViewBag.ErrorMsg = "验证邮件失败，请检查是否正确打开验证邮件";
                return View("Error");
            }
            bool isEmailConfirmed = await Services.SecurityClient.ResetPasswordConfirmEmailAsync(email, code);
            if(!isEmailConfirmed)
            {
                ViewBag.ErrorMsg = "该链接无效或者过期，请重新验证邮件或者重发邮件";
                return View("Error");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            bool isEmailConfirmed = await Services.SecurityClient.ResetPasswordConfirmEmailAsync(model.Email, model.Code);
            if(!isEmailConfirmed)
            {
                AddErrors("修改失败，请重新验证邮件再重置密码");
                return View(model);
            }
            else
            {
                bool isResetSucceeded = await new SecurityClient(new Uri(AppSettingConfig.BaseAddress)).ResetPasswordAsync(model.Email, PasswordHasher.Encrypt(model.Password, true));
                if(isResetSucceeded)
                {
                    return View("ResetPasswordConfirmation");
                }
                else
                {
                    AddErrors("重置密码失败，请稍后重试");
                    return View(model);
                }
            }
        }

        private async Task<string> SendEmailConfirmationTokenAsync(long userId, string toEmailAddress, string subject, EmailConfirmationType type)
        {
            string code = await Services.SecurityClient.GenerateEmailConfirmationTokenIfNotExistAsync(userId, type);
            string callbackUrl = string.Empty;
            if(type == EmailConfirmationType.Register)
            {
                callbackUrl = Url.Action("ConfirmEmail", "Account",
                            new { email = toEmailAddress, code = code }, protocol: Request.Url.Scheme);
            }
            else if(type == EmailConfirmationType.ResetPassword)
            {
                callbackUrl = Url.Action("ResetPassword", "Account",
                            new { email = toEmailAddress, code = code }, protocol: Request.Url.Scheme);
            }
            try
            {
                await SMTPMailClient.SendAsync(subject,
                            "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>", toEmailAddress, "", true);
            }
            catch (System.Net.Mail.SmtpFailedRecipientsException)
            {
                //AddErrors("验证邮件发送失败，请检查邮件是否正确");
            }
            return callbackUrl;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if(Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect("/");
            }
        }

        private void AddErrors(params string[] errors)
        {
            foreach(string error in errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [AllowAnonymous]
        public ActionResult LockedOut()
        {
            return View();
        }
    }
}