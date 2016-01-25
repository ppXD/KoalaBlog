using KoalaBlog.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace KoalaBlog.BLL.Handlers
{
    public abstract class RoleXUserAccountHandlerBase : EntityHandlerBase<RoleXUserAccount>
    {
        private readonly DbContext _dbContext;

        public RoleXUserAccountHandlerBase(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
