using KoalaBlog.Entity.Models;
using KoalaBlog.Principal;
using KoalaBlog.Security.TokenGenerators;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class TokenHandler : TokenHandlerBase
    {
        private readonly DbContext _dbContext;

        public TokenHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 生成Bearer Token
        /// </summary>
        /// <param name="userAccountId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="expirationDate">过期时间</param>
        /// <param name="isSlidingExpiration">是否滑动过期</param>
        /// <returns></returns>
        public async Task<Token> GenerateBearerTokenAsync(long userAccountId, DateTime? expirationDate = null, bool isSlidingExpiration = true)
        {
            Token bearerToken = new Token()
            {
                UserAccountID = userAccountId,
                AccessToken = new Base64TokenGenerator().GenerateToken(),
                TokenType = Entity.Models.Enums.TokenType.Bearer,
                ExpirationDate = expirationDate ?? DateTime.Now.AddDays(1),
                IsSlidingExpiration = isSlidingExpiration,
                IsRevoked = false
            };

            Add(bearerToken);

            return await SaveChangesAsync() > 0 ? bearerToken : null;
        }

        /// <summary>
        /// 验证Bearer Token
        /// </summary>
        /// <param name="userAccountId">用户ID</param>
        /// <param name="accessToken">令牌</param>
        /// <returns></returns>
        public async Task<IPrincipal> AuthenticateBearerTokenAsync(string accessToken)
        {
            //1. 根据条件获取Token对象。
            Token bearerToken = await Fetch(x => x.AccessToken == accessToken && !x.IsRevoked && x.ExpirationDate > DateTime.Now).SingleOrDefaultAsync();

            if(bearerToken != null)
            {
                //2. 如果Token对象不为空，则为Token验证成功，建立Principal。
                KoalaBlogIdentityObject identityObj = new KoalaBlogIdentityObject();

                UserAccountXPersonHandler uaxpHandler = new UserAccountXPersonHandler(_dbContext);

                //3. 获取UserAccountXPerson对象。
                UserAccountXPerson uaxp = await uaxpHandler.LoadByUserAccountIDIncludeUserAccountAndPersonAsync(bearerToken.UserAccountID);

                if(uaxp != null)
                {
                    if (uaxp.UserAccount != null)
                    {
                        identityObj.UserID = uaxp.UserAccount.ID;
                        identityObj.UserName = uaxp.UserAccount.UserName;
                        identityObj.Email = uaxp.UserAccount.Email;
                        identityObj.Status = uaxp.UserAccount.Status;
                    }
                    if (uaxp.Person != null)
                    {
                        identityObj.PersonID = uaxp.Person.ID;
                        identityObj.PersonNickName = uaxp.Person.NickName;
                        identityObj.Introduction = uaxp.Person.Introduction;
                    }
                }
                else
                {
                    UserAccountHandler uaHandler = new UserAccountHandler(_dbContext);

                    //4. 如果UserAccountXPerson对象为空，意味着可能是用户注册还没完成，则根据用户名获取UserAccount对象，赋值IdentityObject通用Property。
                    UserAccount userAccount = await uaHandler.GetByIdAsync(bearerToken.UserAccountID);

                    if (userAccount != null)
                    {
                        identityObj.UserID = userAccount.ID;
                        identityObj.UserName = userAccount.UserName;
                        identityObj.Email = userAccount.Email;
                        identityObj.Status = userAccount.Status;
                    }
                }

                KoalaBlogIdentity identity = new KoalaBlogIdentity(identityObj);
                KoalaBlogPrincipal principal = new KoalaBlogPrincipal(identity);

                return principal;
            }

            return null;
        }
    }
}
