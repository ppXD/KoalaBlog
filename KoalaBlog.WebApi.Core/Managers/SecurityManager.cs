using KoalaBlog.BLL.Handlers;
using KoalaBlog.DAL;
using KoalaBlog.Entity.Models;
using KoalaBlog.Entity.Models.Enums;
using KoalaBlog.Framework.Enums;
using KoalaBlog.Framework.Security;
using KoalaBlog.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.WebApi.Core.Managers
{
    public class SecurityManager : ManagerBase
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<Tuple<KoalaBlogIdentityObject, SignInStatus, string>> SignInAsync(string userName, string password, bool isPersistent)
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                UserAccountHandler uaHandler = new UserAccountHandler(dbContext);

                return await uaHandler.SignInAsync(userName, password, isPersistent);
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public async Task<SignOutStatus> SignOutAsync(string userName, string token)
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                UserAccountHandler uaHandler = new UserAccountHandler(dbContext);

                return await uaHandler.SignOutAsync(userName, token);
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        public async Task<Tuple<UserAccount, RegisterStatus>> RegisterAsync(string userName, string password, string email)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                UserAccountHandler uaHandler = new UserAccountHandler(dbContext);

                RegisterStatus registerStatus = RegisterStatus.Failure;

                UserAccount registerUser = await uaHandler.CreateAsync(userName, password, email);

                if (registerUser != null)
                {
                    registerStatus = RegisterStatus.Succeeded;
                }

                return new Tuple<UserAccount, RegisterStatus>(registerUser, registerStatus);
            }
        }

        /// <summary>
        /// 重设密码
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                UserAccountHandler uaHandler = new UserAccountHandler(dbContext);

                return await uaHandler.ResetPasswordAsync(email, newPassword);
            }
        }

        /// <summary>
        /// 授权判断
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="roleOrPermissionNames">角色或者权限名称</param>
        /// <returns></returns>
        public async Task<bool> IsUserInRoleAsync(string userName, string[] roleOrPermissionNames)
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                UserAccountHandler uaHandler = new UserAccountHandler(dbContext);

                return await uaHandler.IsUserInRoleAsync(userName, roleOrPermissionNames);
            }
        }

        /// <summary>
        /// 根据UserName获取KoalaBlogIdentityObject
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<KoalaBlogIdentityObject> GetIdentityObjectAsync()
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                UserAccountXPersonHandler uaxpHandler = new UserAccountXPersonHandler(dbContext);

                if(CurrentThreadIdentityObject != null)
                {
                    //1. 根据用户名获取UserAccountXPerson对象。
                    UserAccountXPerson uaxp = await uaxpHandler.LoadByUserNameAsync(CurrentThreadIdentityObject.UserName);

                    if (uaxp != null)
                    {
                        KoalaBlogIdentityObject identityObject = new KoalaBlogIdentityObject();

                        if (uaxp.UserAccount != null)
                        {
                            identityObject.UserID = uaxp.UserAccount.ID;
                            identityObject.UserName = uaxp.UserAccount.UserName;
                            identityObject.Email = uaxp.UserAccount.Email;
                            identityObject.Status = uaxp.UserAccount.Status;
                        }
                        if (uaxp.Person != null)
                        {
                            AvatarHandler avatarHandler = new AvatarHandler(dbContext);

                            Avatar avatar = await avatarHandler.GetActiveAvatarByPersonId(uaxp.Person.ID);

                            identityObject.PersonID = uaxp.Person.ID;
                            identityObject.PersonNickName = uaxp.Person.NickName;
                            identityObject.Introduction = uaxp.Person.Introduction;

                            if (avatar != null)
                            {
                                identityObject.AvatarUrl = avatar.AvatarPath;
                            }
                        }

                        return identityObject;
                    }
                    else
                    {
                        UserAccountHandler uaHandler = new UserAccountHandler(dbContext);

                        //2. 如果UserAccountXPerson对象为空，意味着可能是用户注册还没完成，则根据用户名获取UserAccount对象，赋值IdentityObject通用Property。
                        UserAccount userAccount = await uaHandler.GetByUserNameAsync(CurrentThreadIdentityObject.UserName);

                        if (userAccount != null)
                        {
                            KoalaBlogIdentityObject identityObject = new KoalaBlogIdentityObject()
                            {
                                UserID = userAccount.ID,
                                UserName = userAccount.UserName,
                                Email = userAccount.Email,
                                Status = userAccount.Status
                            };
                            return identityObject;
                        }
                    }
                }     

                return null;
            }
        }

        /// <summary>
        /// 验证Bearer Token
        /// </summary>
        /// <param name="userAccountId">用户ID</param>
        /// <param name="accessToken">令牌</param>
        /// <returns></returns>
        public async Task<IPrincipal> AuthenticateBearerTokenAsync(string accessToken)
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                TokenHandler tokenHandler = new TokenHandler(dbContext);

                return await tokenHandler.AuthenticateBearerTokenAsync(accessToken);
            }
        }

        public async Task<UserAccount> GetSafeUserAccountByEmailAsync(string email)
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                UserAccountHandler uaHandler = new UserAccountHandler(dbContext);
                UserAccount user = await uaHandler.GetByEmailAsync(email);
                if(user != null)
                {
                    user.CreatedBy = 0;
                    user.CreatedDate = DateTime.MinValue;
                    user.LastModifiedBy = 0;
                    user.LastModifiedDate = DateTime.MinValue;
                    user.UserName = string.Empty;
                    user.Password = string.Empty;
                    user.Status = string.Empty;
                    user.LastLogon = DateTime.MinValue;
                }
                return user;
            }
        }

        /// <summary>
        /// 生成邮件验证码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="type">验证码类型</param>
        /// <returns></returns>
        public async Task<string> GenerateEmailConfirmationTokenIfNotExistAsync(long userId, EmailConfirmationType type)
        {
            using(KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                EmailConfirmationHandler emailConfirmationHandler = new EmailConfirmationHandler(dbContext);

                return await emailConfirmationHandler.GenerateEmailConfirmationTokenIfNotExistAsync(userId, type);
            }
        }

        /// <summary>
        /// 确认邮件
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public async Task<bool> ConfirmEmailAsync(string email, string code)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                EmailConfirmationHandler emailConfirmationHandler = new EmailConfirmationHandler(dbContext);

                return await emailConfirmationHandler.ConfirmEmailAsync(email, code);
            }
        }

        /// <summary>
        /// 重设密码Email验证
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public async Task<bool> ResetPasswordConfirmEmailAsync(string email, string code)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                EmailConfirmationHandler emailConfirmationHandler = new EmailConfirmationHandler(dbContext);

                return await emailConfirmationHandler.ResetPasswordConfirmEmailAsync(email, code);
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logLevel">日志级别</param>
        /// <param name="logSourceObj">日志来源对象</param>
        /// <param name="logMsg">日志信息</param>
        /// <returns></returns>
        public async Task<bool> WriteLogAsync(LogLevel logLevel, string logSourceObj, string logMsg)
        {
            using (KoalaBlogDbContext dbContext = new KoalaBlogDbContext())
            {
                LogHandler logHandler = new LogHandler(dbContext);

                return await logHandler.WriteAsync(logLevel, logSourceObj, logMsg);
            }
        }
    }
}
