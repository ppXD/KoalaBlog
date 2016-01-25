using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public abstract class LogHandlerBase
    {
        private readonly DbContext _dbContext;

        public LogHandlerBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
