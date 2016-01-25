using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using KoalaBlog.Entity.Models;

namespace KoalaBlog.BLL.Handlers
{
    public class RoleXUserAccountHandler : RoleXUserAccountHandlerBase
    {
        private readonly DbContext _dbContext;

        public RoleXUserAccountHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public List<RoleXUserAccount> LoadByUserName(string userName)
        {
            List<RoleXUserAccount> rxuaList = null;

            rxuaList = Entities.Include(x => x.Role)
                               .Where(x => x.UserAccount.UserName == userName)
                               .ToList();

            return rxuaList ?? new List<RoleXUserAccount>();
        }

        public async Task<List<RoleXUserAccount>> LoadByUserNameAsync(string userName)
        {
            List<RoleXUserAccount> rxuaList = null;

            rxuaList = await Entities.Include(x => x.Role)
                                     .Where(x => x.UserAccount.UserName == userName)
                                     .ToListAsync();

            return rxuaList ?? new List<RoleXUserAccount>();
        }

    }
}
