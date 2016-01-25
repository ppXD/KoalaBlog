using KoalaBlog.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace KoalaBlog.BLL.Handlers
{
    public abstract class UserAccountHandlerBase : EntityHandlerBase<UserAccount>
    {
        private readonly DbContext _dbContext;

        public UserAccountHandlerBase(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
