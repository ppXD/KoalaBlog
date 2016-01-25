using KoalaBlog.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public abstract class TokenHandlerBase : EntityHandlerBase<Token>
    {
        private readonly DbContext _dbContext;

        public TokenHandlerBase(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
