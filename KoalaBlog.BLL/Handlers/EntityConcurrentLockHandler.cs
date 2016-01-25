using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class EntityConcurrentLockHandler : EntityConcurrentLockHandlerBase
    {
        private readonly DbContext _dbContext;

        public EntityConcurrentLockHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
