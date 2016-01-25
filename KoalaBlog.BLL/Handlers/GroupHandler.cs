using KoalaBlog.Entity.Models;
using KoalaBlog.Entity.Models.Enums;
using KoalaBlog.Framework.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class GroupHandler : GroupHandlerBase
    {
        private readonly DbContext _dbContext;

        public GroupHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Group> CreateGroupAsync(long personId, string groupName, GroupType groupType)
        {
            AssertUtil.Waterfall()
                .AreBigger(personId, 0, "personId must be greater than zero")
                .IsNotNull(groupType, "groupType can't be null")
                .Done();

            Group group = new Group()
            {
                PersonID = personId,
                Name = groupName,
                Type = groupType
            };

            this.Add(group);

            return await SaveChangesAsync() > 0 ? group : null;
        }
    }
}
