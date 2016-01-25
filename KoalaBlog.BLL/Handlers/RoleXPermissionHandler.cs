using KoalaBlog.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.BLL.Handlers
{
    public class RoleXPermissionHandler : RoleXPermissionHandlerBase
    {
        private readonly DbContext _dbContext;

        public RoleXPermissionHandler(DbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public List<RoleXPermission> LoadByRoleNameAndPermissionName(string roleName, string permissionName)
        {
            List<RoleXPermission> rxpList = null;

            rxpList = Fetch(x => x.Role.Name == roleName && x.Permission.Name == permissionName).ToList();

            return rxpList ?? new List<RoleXPermission>();
        }

        public async Task<List<RoleXPermission>> LoadByRoleNameAndPermissionNameAsync(string roleName, string permissionName)
        {
            List<RoleXPermission> rxpList = null;

            rxpList = await Fetch(x => x.Role.Name == roleName && x.Permission.Name == permissionName).ToListAsync();

            return rxpList ?? new List<RoleXPermission>();
        }

        public async Task<List<RoleXPermission>> LoadByRoleNameAsync(string roleName)
        {
            List<RoleXPermission> rxpList = null;

            rxpList = await Include(x => x.Permission).Where(x => x.Role.Name == roleName).ToListAsync();

            return rxpList ?? new List<RoleXPermission>();
        }
    }
}
