using KoalaBlog.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public abstract class UserAccountXPersonHandlerBase : EntityHandlerBase<UserAccountXPerson>
    {
        private readonly DbContext _dbContext;

        public UserAccountXPersonHandlerBase(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
