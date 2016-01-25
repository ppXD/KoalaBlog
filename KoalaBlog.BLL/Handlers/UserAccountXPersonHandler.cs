using KoalaBlog.Entity.Models;
using KoalaBlog.Framework.Enums;
using KoalaBlog.Framework.Security;
using KoalaBlog.Framework.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class UserAccountXPersonHandler : UserAccountXPersonHandlerBase
    {
        private readonly DbContext _dbContext;

        public UserAccountXPersonHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 根据UserAccountID获取UserAccountXPerson对象（Join Person）
        /// </summary>
        /// <param name="userAccountID">用户ID</param>
        /// <returns></returns>
        public async Task<UserAccountXPerson> LoadByUserAccountIDAsync(long userAccountID)
        {
            UserAccountXPerson uaxp = null;

            uaxp = await Entities.Include(x => x.Person)
                                 .Where(x => x.UserAccountID == userAccountID).SingleOrDefaultAsync();

            return uaxp;
        }

        /// <summary>
        /// 根据UserName获取UserAccountXPerson对象（Join Person And UserAccount）
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public async Task<UserAccountXPerson> LoadByUserNameAsync(string userName)
        {
            return await Include(x => x.UserAccount, x => x.Person).Where(x => x.UserAccount.UserName == userName).SingleOrDefaultAsync();
        }

        /// <summary>
        /// 根据UserAccountID获取UserAccountXPerson对象（Join Person and UserAccount）
        /// </summary>
        /// <param name="userAccountID">用户ID</param>
        public async Task<UserAccountXPerson> LoadByUserAccountIDIncludeUserAccountAndPersonAsync(long userAccountID)
        {
            UserAccountXPerson uaxp = null;

            uaxp = await Entities.Include(x => x.Person).Include(x => x.UserAccount)
                                 .Where(x => x.UserAccountID == userAccountID).SingleOrDefaultAsync();

            return uaxp;
        }
    }
}
