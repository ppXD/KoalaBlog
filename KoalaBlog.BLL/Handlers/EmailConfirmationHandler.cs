using KoalaBlog.Entity.Models;
using KoalaBlog.Entity.Models.Enums;
using KoalaBlog.Framework.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace KoalaBlog.BLL.Handlers
{
    public class EmailConfirmationHandler : EmailConfirmationHandlerBase
    {
        private readonly DbContext _dbContext;

        public EmailConfirmationHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GenerateEmailConfirmationTokenIfNotExistAsync(long userId, EmailConfirmationType type)
        {
            AssertUtil.AreBigger(userId, 0, "用户不存在");

            EmailConfirmation ec = await Fetch(x => x.UserAccountID == userId && x.Type == type).FirstOrDefaultAsync();
            if(ec != null)
            {
                return ec.Code;
            }
            else
            {
                EmailConfirmation emailConfirmation = new EmailConfirmation()
                {
                    UserAccountID = userId,
                    Code = Guid.NewGuid().ToString(),
                    Type = type
                };

                Add(emailConfirmation);
                return await SaveChangesAsync() > 0 ? emailConfirmation.Code : string.Empty;
            }
        }

        public async Task<bool> ConfirmEmailAsync(string email, string code)
        {
            AssertUtil.Waterfall()
                .NotNullOrWhiteSpace(email, "邮箱不能为空")
                .NotNullOrWhiteSpace(code, "验证码不能为空")
                .IsValidEmail(email, "邮箱地址不正确")
                .Done();

            UserAccountHandler uaHandler = new UserAccountHandler(_dbContext);
            PersonHandler perHandler = new PersonHandler(_dbContext);
            AvatarHandler avatarHandler = new AvatarHandler(_dbContext);

            UserAccount user = await uaHandler.GetByEmailAsync(email);

            if (user != null)
            {
                //1. 判断验证码是否匹配。
                bool isMatched = await Entities.AnyAsync(x => x.UserAccountID == user.ID && x.Code == code);

                if (isMatched)
                {
                    //这里需要用事务来保证执行成功。
                    using(TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        //2. 匹配则修改邮件验证为True。
                        user.EmailConfirmed = true;

                        uaHandler.MarkAsModified(user);

                        bool isSucceed = await SaveChangesAsync() > 0;

                        //3. 同时生成UserAccountXPerson记录。
                        if (isSucceed)
                        {
                            Person per = await perHandler.CreatePersonAsync(user);

                            //4. 生成默认Avatar。
                            await avatarHandler.CreateDefaultAvatar(per.ID);
                        }

                        transactionScope.Complete();

                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> ResetPasswordConfirmEmailAsync(string email, string code)
        {
            AssertUtil.Waterfall()
                .NotNullOrWhiteSpace(email, "邮箱不能为空")
                .NotNullOrWhiteSpace(code, "验证码不能为空")
                .IsValidEmail(email, "邮箱地址不正确")              
                .Done();

            UserAccountHandler uaHandler = new UserAccountHandler(_dbContext);

            UserAccount user = await uaHandler.GetByEmailAsync(email);
            bool isMatched = false;
            
            if(user != null)
            {
                isMatched = await Entities.AnyAsync(x => x.UserAccountID == user.ID && x.Code == code && x.Type == EmailConfirmationType.ResetPassword);
            }
            return isMatched;
        }
    }
}
