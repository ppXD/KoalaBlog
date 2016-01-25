using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class EducationHandler : EducationHandlerBase
    {
        private readonly DbContext _dbContext;

        public EducationHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
