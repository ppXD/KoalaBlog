using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class SchoolCategoryHandler : SchoolCategoryHandlerBase
    {
        private readonly DbContext _dbContext;

        public SchoolCategoryHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
