using KoalaBlog.Entity.Models;
using KoalaBlog.Framework.Enums;
using KoalaBlog.WebApi.Models;
using KoalaBlog.WebApi.Core.Managers;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using KoalaBlog.Entity.Models.Enums;

namespace KoalaBlog.WebApi.Controllers
{
    [RoutePrefix("koala/api/security")]
    public class SecurityController : BaseApiController
    {
        // POST koala/api/security/SignIn
        [Route("SignIn"), HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SignInAsync([FromBody]SignInBindingModel model)
        {
            var result = await new SecurityManager().SignInAsync(model.UserName, model.Password, model.IsPersistent);

            return Ok(result);
        }

        // GET koala/api/security/SignOut/username
        [Route("SignOut"), HttpPost]
        public async Task<SignOutStatus> SignOutAsync([FromBody]SignOutBindingModel model)
        {
            return await new SecurityManager().SignOutAsync(model.UserName, model.Token);
        }

        // POST koala/api/security/Register
        [Route("Register"), HttpPost]
        [AllowAnonymous]
        public async Task<Tuple<UserAccount, RegisterStatus>> RegisterAsync([FromBody]RegisterBindingModel model)
        {
            return await new SecurityManager().RegisterAsync(model.UserName, model.Password, model.Email);
        }

        // GET koala/api/security/GetIdentityObject/userName
        [Route("GetIdentityObject"), HttpGet]
        public async Task<IHttpActionResult> GetIdentityObjectAsync()
        {
            var result = await new SecurityManager().GetIdentityObjectAsync();

            return Ok(result);
        }

        // GET koala/api/security/GetSafeUserAccountByEmail/email
        [Route("GetSafeUserAccountByEmail/{email}"), HttpGet]
        [AllowAnonymous]
        public async Task<UserAccount> GetSafeUserAccountByEmailAsync(string email)
        {
            return await new SecurityManager().GetSafeUserAccountByEmailAsync(email);
        }

        // POST koala/api/security/IsUserInRole
        [Route("IsUserInRole"), HttpPost]
        public async Task<bool> IsUserInRoleAsync([FromBody]IsUserInRoleBindingModel model)
        {
            return await new SecurityManager().IsUserInRoleAsync(model.UserName, model.RolesOrPermissionsName);
        }

        // POST koala/api/security/ResetPassword
        [Route("ResetPassword"), HttpPost]
        [AllowAnonymous]
        public async Task<bool> ResetPasswordAsync([FromBody]ResetPasswordBindingModel model)
        {
            return await new SecurityManager().ResetPasswordAsync(model.Email, model.NewPassword);
        }

        // GET koala/api/security/GenerateEmailConfirmationTokenIfNotExist/userId/type
        [Route("GenerateEmailConfirmationTokenIfNotExist/{userId}/{type}"), HttpGet]
        [AllowAnonymous]
        public async Task<string> GenerateEmailConfirmationTokenIfNotExistAsync(long userId, EmailConfirmationType type)
        {
            return await new SecurityManager().GenerateEmailConfirmationTokenIfNotExistAsync(userId, type);
        }

        // GET koala/api/security/ConfirmEmail/email/code
        [Route("ConfirmEmail/{email}/{code}"), HttpGet]
        [AllowAnonymous]
        public async Task<bool> ConfirmEmailAsync(string email, string code)
        {
            return await new SecurityManager().ConfirmEmailAsync(email, code);
        }

        // GET koala/api/security/ResetPasswordConfirmEmail/email/code
        [Route("ResetPasswordConfirmEmail/{email}/{code}"), HttpGet]
        [AllowAnonymous]
        public async Task<bool> ResetPasswordConfirmEmailAsync(string email, string code)
        {
            return await new SecurityManager().ResetPasswordConfirmEmailAsync(email, code);
        }
    }
}