using KoalaBlog.Entity.Models.Enums;
using KoalaBlog.Framework.Enums;
using KoalaBlog.Framework.Extensions;
using KoalaBlog.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.ApiClient
{
    public class SecurityClient : BaseClient
    {
        public SecurityClient(Uri baseEndpoint)
            : base(baseEndpoint)
        {
        }

        public async Task<Tuple<KoalaBlogIdentityObject, SignInStatus, string>> SignInAsync(string userName, string password, bool isPersistent)
        {
            var postModel = new
            {
                UserName = userName,
                Password = password,
                IsPersistent = isPersistent
            };

            return await PostAsync<Tuple<KoalaBlogIdentityObject, SignInStatus, string>>(RelativePaths.SignIn, postModel);
        }

        public async Task<SignOutStatus> SignOutAsync(string userName, string token)
        {
            var postModel = new
            {
                UserName = userName,
                Token = token
            };

            return await PostAsync<SignOutStatus>(RelativePaths.SignOut, postModel);
        }

        public async Task<Tuple<object, RegisterStatus>> RegisterAsync(string userName, string password, string email)
        {
            var postModel = new
            {
                UserName = userName,
                Password = password,
                Email = email
            };

            return await PostAsync<Tuple<object, RegisterStatus>>(RelativePaths.Register, postModel);
        }

        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var postModel = new
            {                
                Email = email,
                NewPassword = newPassword
            };

            return await PostAsync<bool>(RelativePaths.ResetPassword, postModel);
        }

        public async Task<object> GetSafeUserAccountByEmailAsync(string email)
        {
            return await GetAsync<object>(RelativePaths.GetSafeUserAccountByEmail.Link(email));
        }

        public async Task<string> GenerateEmailConfirmationTokenIfNotExistAsync(long userId, EmailConfirmationType type)
        {
            return await GetAsync<string>(RelativePaths.GenerateEmailConfirmationTokenIfNotExist.Link(userId, type));
        }

        public async Task<bool> ConfirmEmailAsync(string email, string code)
        {
            return await GetAsync<bool>(RelativePaths.ConfirmEmail.Link(email, code));
        }

        public async Task<bool> ResetPasswordConfirmEmailAsync(string email, string code)
        {
            return await GetAsync<bool>(RelativePaths.ResetPasswordConfirmEmail.Link(email, code));
        }

        public KoalaBlogIdentityObject GetIdentityObj()
        {
            return GetSync<KoalaBlogIdentityObject>(RelativePaths.GetIdentityObject);
        }

        public bool IsUserInRole(string userName, string[] rolesOrPermissionsName)
        {
            var postModel = new
            {
                UserName = userName,
                RolesOrPermissionsName = rolesOrPermissionsName
            };

            return PostSync<bool, object>(RelativePaths.IsUserInRole, postModel);
        }

        protected class RelativePaths
        {
            private const string Prefix = "koala/api/security";

            public const string SignIn = Prefix + "/SignIn";
            public const string SignOut = Prefix + "/SignOut";
            public const string Register = Prefix + "/Register";
            public const string GetIdentityObject = Prefix + "/GetIdentityObject";
            public const string GetSafeUserAccountByEmail = Prefix + "/GetSafeUserAccountByEmail";
            public const string IsUserInRole = Prefix + "/IsUserInRole";
            public const string GenerateEmailConfirmationTokenIfNotExist = Prefix + "/GenerateEmailConfirmationTokenIfNotExist";
            public const string ConfirmEmail = Prefix + "/ConfirmEmail";
            public const string ResetPasswordConfirmEmail = Prefix + "/ResetPasswordConfirmEmail";
            public const string ResetPassword = Prefix + "/ResetPassword";
        }
    }
}
