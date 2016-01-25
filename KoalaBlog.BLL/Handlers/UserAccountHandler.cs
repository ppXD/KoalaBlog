using KoalaBlog.Entity.Models;
using KoalaBlog.Framework.Common;
using KoalaBlog.Framework.Enums;
using KoalaBlog.Framework.Exceptions;
using KoalaBlog.Framework.Security;
using KoalaBlog.Framework.Util;
using KoalaBlog.Principal;
using KoalaBlog.Security;
using KoalaBlog.Security.TokenGenerators;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class UserAccountHandler : UserAccountHandlerBase
    {
        private readonly DbContext _dbContext;

        public UserAccountHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="email">Email</param>
        /// <returns></returns>
        public async Task<UserAccount> CreateAsync(string userName, string password, string email)
        {
            if(!CommonHelper.IsValidEmail(email))
            {
                throw new DisplayableException("邮箱地址不正确");
            }
            if(await GetByUserNameAsync(userName) != null)
            {
                throw new DisplayableException("该用户名已被注册");
            }
            if(await GetByEmailAsync(email) != null)
            {
                throw new DisplayableException("该邮箱地址已被注册");
            }

            string saltKey = KoalaBlogSecurityManager.CreateSaltKey(5);

            UserAccount ua = new UserAccount()
            {
                LastLogon = DateTime.Now,
                UserName = userName,
                Password = KoalaBlogSecurityManager.CreatePasswordHash(password, saltKey),
                PasswordSalt = saltKey,
                Email = email,
                EmailConfirmed = false,
                Status = UserAccount.STATUS_ACTIVE,
                IsOnline = false
            };
            Add(ua);

            return await SaveChangesAsync() > 0 ? ua : null;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="hashPassword">密码</param>
        /// <returns></returns>
        public async Task<Tuple<KoalaBlogIdentityObject, SignInStatus, string>> SignInAsync(string userName, string password, bool isPersistent)
        {
            KoalaBlogIdentityObject identityObject = new KoalaBlogIdentityObject();

            var userAccount = await GetByUserNameAsync(userName);

            if (userAccount != null)
            {
                //1. 设置Common Property。
                identityObject.UserID = userAccount.ID;
                identityObject.UserName = userAccount.UserName;
                identityObject.Email = userAccount.Email;
                identityObject.Status = userAccount.Status;

                bool isEmailConfirmed = userAccount.EmailConfirmed;
                if (!isEmailConfirmed)
                {
                    return new Tuple<KoalaBlogIdentityObject, SignInStatus, string>(identityObject, SignInStatus.NotYetEmailConfirmed, string.Empty);
                }
                else
                {
                    //2. 如果用户已经邮件验证完成则获取Person对象。
                    UserAccountXPersonHandler uaxpHandler = new UserAccountXPersonHandler(_dbContext);

                    UserAccountXPerson uaxp = await uaxpHandler.LoadByUserAccountIDAsync(userAccount.ID);

                    if (uaxp != null && uaxp.Person != null)
                    {
                        identityObject.PersonID = uaxp.Person.ID;
                        identityObject.PersonNickName = uaxp.Person.NickName;
                        identityObject.Introduction = uaxp.Person.Introduction;
                    }

                    bool isLockedOut = userAccount.Status == UserAccount.STATUS_SUSPENDED;
                    if (isLockedOut)
                    {
                        return new Tuple<KoalaBlogIdentityObject, SignInStatus, string>(identityObject, SignInStatus.LockedOut, string.Empty);
                    }

                    string pwd = KoalaBlogSecurityManager.CreatePasswordHash(password, userAccount.PasswordSalt);

                    bool isValid = pwd == userAccount.Password;
                    if (isValid)
                    {
                        userAccount.LastLogon = DateTime.Now;
                        userAccount.IsOnline = true;

                        await ModifyAsync(userAccount);

                        //3. 如果登录成功则生成一个Bearer Token。
                        TokenHandler tokenHandler = new TokenHandler(_dbContext);

                        DateTime? expirationDate = isPersistent ? DateTime.MaxValue : (DateTime?)null;
                        
                        Token bearerToken = await tokenHandler.GenerateBearerTokenAsync(userAccount.ID, expirationDate);

                        return new Tuple<KoalaBlogIdentityObject, SignInStatus, string>(identityObject, SignInStatus.Succeeded, bearerToken.AccessToken);
                    }
                    else
                    {
                        return new Tuple<KoalaBlogIdentityObject, SignInStatus, string>(identityObject, SignInStatus.WrongPassword, string.Empty);
                    }
                }
            }
            return new Tuple<KoalaBlogIdentityObject, SignInStatus, string>(identityObject, SignInStatus.Failure, string.Empty);
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public async Task<SignOutStatus> SignOutAsync(string userName, string token)
        {
            UserAccount user = await GetByUserNameAsync(userName);

            if(user != null)
            {
                user.IsOnline = false;

                //1. Set user account offline
                MarkAsModified(user);

                #region Revoked user account token.

                TokenHandler tokenHandler = new TokenHandler(_dbContext);

                Token accessToken = await tokenHandler.Entities.SingleOrDefaultAsync(x => x.AccessToken == token && x.UserAccountID == user.ID);

                accessToken.IsRevoked = true;

                tokenHandler.MarkAsModified(accessToken);

                #endregion

                return await SaveChangesAsync() > 0 ? SignOutStatus.Succeeded : SignOutStatus.Failure;
            }

            return SignOutStatus.Failure;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            UserAccount user = await GetByEmailAsync(email);

            if(user != null)
            {
                string saltKey = KoalaBlogSecurityManager.CreateSaltKey(5);

                user.PasswordSalt = saltKey;
                user.Password = KoalaBlogSecurityManager.CreatePasswordHash(newPassword, saltKey);

                MarkAsModified(user);

                return await SaveChangesAsync() > 0;
            }
            return false;
        }

        /// <summary>
        /// 授权判断
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="rolesOrPermissionsName">角色或者权限名称</param>
        /// <returns></returns>
        public async Task<bool> IsUserInRoleAsync(string userName, string[] rolesOrPermissionsName)
        {
            if (rolesOrPermissionsName == null || rolesOrPermissionsName.Length == 0)
            {
                throw new ArgumentNullException("roleOrPermissionNames");
            }

            int calculateCount = rolesOrPermissionsName.Length;

            RoleXUserAccountHandler rxuaHandler = new RoleXUserAccountHandler(_dbContext);
            RoleXPermissionHandler rxpHandler = new RoleXPermissionHandler(_dbContext);
            UserAccountXPermissionHandler uaxpHandler = new UserAccountXPermissionHandler(_dbContext);

            List<RoleXUserAccount> rxuaList = await rxuaHandler.LoadByUserNameAsync(userName);            

            //first step, check the user belongs to a role
            foreach (RoleXUserAccount rxua in rxuaList)
            {
                if(calculateCount == 0)
                {
                    break;
                }

                foreach (string roleOrPermissionName in rolesOrPermissionsName)
                {
                    if (rxua.Role.Name.ToUpper() == roleOrPermissionName.ToUpper())
                    {
                        calculateCount--;
                        break;
                    }
                }

                if(calculateCount > 0)
                {
                    List<RoleXPermission> rxpList = await rxpHandler.LoadByRoleNameAsync(rxua.Role.Name);

                    foreach (RoleXPermission rxp in rxpList)
                    {
                        if(calculateCount == 0)
                        {
                            break;
                        }

                        foreach (string roleOrPermissionName in rolesOrPermissionsName)
                        {
                            if (rxp.Permission.Name == roleOrPermissionName.ToUpper())
                            {
                                calculateCount--;
                                break;
                            }
                        }
                    }
                }
            }
            //second step, check the user has permission
            if(calculateCount > 0)
            {
                List<UserAccountXPermission> uaxpList = await uaxpHandler.LoadByUserNameAsync(userName);

                foreach (UserAccountXPermission uaxp in uaxpList)
                {
                    if(calculateCount == 0)
                    {
                        break;
                    }

                    foreach (string roleOrPermissionName in rolesOrPermissionsName)
                    {
                        if (uaxp.Permission.Name == roleOrPermissionName.ToUpper())
                        {
                            calculateCount--;
                            break;
                        }
                    }
                }
            }
            
            return calculateCount == 0;
        }

        public async Task<bool> ModifyAsync(UserAccount ua)
        {
            MarkAsModified(ua);

            await SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// 根据UserName获取UserAccount对象
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public async Task<UserAccount> GetByUserNameAsync(string userName)
        {
            return await Fetch(x => x.UserName == userName).SingleOrDefaultAsync();
        }

        /// <summary>
        /// 根据Email获取UserAccount对象
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        public async Task<UserAccount> GetByEmailAsync(string email)
        {
            return await Fetch(x => x.Email == email).SingleOrDefaultAsync();
        }

        /// <summary>
        /// 根据UserName判断UserAccount对象是否已经邮件验证
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public async Task<bool> IsEmailConfirmedAsync(string userName)
        {
            UserAccount ua = await GetByUserNameAsync(userName);

            return ua.EmailConfirmed;
        }

        /// <summary>
        /// 根据UserName判断UserAccount对象是否被锁定
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public async Task<bool> IsLockedOutAsync(string userName)
        {
            return ((await GetByUserNameAsync(userName)).Status == UserAccount.STATUS_SUSPENDED);
        }

        /// <summary>
        /// 判断用户名是否存在重复
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public async Task<bool> IsUserNameDuplicatedAsync(string userName)
        {
            if(await CountAsync(x => x.UserName == userName) > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断邮件是否存在重复
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        public async Task<bool> IsEmailDuplicatedAsync(string email)
        {
            if(await CountAsync(x => x.Email == email) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
