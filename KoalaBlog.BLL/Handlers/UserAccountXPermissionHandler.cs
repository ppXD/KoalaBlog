using KoalaBlog.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class UserAccountXPermissionHandler : UserAccountXPermissionHandlerBase
    {
        private readonly DbContext _dbContext;

        public UserAccountXPermissionHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public List<UserAccountXPermission> LoadByUserNameAndPermissionName(string userName, string permissionName)
        {
            List<UserAccountXPermission> uaxpList = null;

            uaxpList = Fetch(x => x.UserAccount.UserName == userName && x.Permission.Name == permissionName).ToList();

            return uaxpList ?? new List<UserAccountXPermission>();
        }

        public async Task<List<UserAccountXPermission>> LoadByUserNameAndPermissionNameAsync(string userName, string permissionName)
        {
            List<UserAccountXPermission> uaxpList = null;

            uaxpList = await Fetch(x => x.UserAccount.UserName == userName && x.Permission.Name == permissionName).ToListAsync();

            return uaxpList ?? new List<UserAccountXPermission>();
        }

        public async Task<List<UserAccountXPermission>> LoadByUserNameAsync(string userName)
        {
            List<UserAccountXPermission> uaxpList = null;

            uaxpList = await Include(x => x.Permission).Where(x => x.UserAccount.UserName == userName).ToListAsync();

            return uaxpList ?? new List<UserAccountXPermission>();
        }
    }
}