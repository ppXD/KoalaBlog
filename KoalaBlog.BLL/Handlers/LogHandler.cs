using KoalaBlog.Entity.Models;
using KoalaBlog.Entity.Models.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class LogHandler : LogHandlerBase
    {
        private readonly DbContext _dbContext;

        public LogHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> WriteAsync(LogLevel logLevel, string logSourceObj, string logMsg)
        {            
            try
            {
                Log log = new Log();
                log.LogTime = DateTime.Now;
                log.LogLevel = logLevel;
                log.LogSource = logSourceObj;
                log.LogMessage = logMsg;
                
                _dbContext.Set<Log>().Add(log);

                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
