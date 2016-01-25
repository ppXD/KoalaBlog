using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class Permission : EntityBase
    {
        public Permission()
        {
            this.RoleXPermissions = new List<RoleXPermission>();
            this.UserAccountXPermissions = new List<UserAccountXPermission>();
        }

        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<RoleXPermission> RoleXPermissions { get; set; }
        public virtual ICollection<UserAccountXPermission> UserAccountXPermissions { get; set; }
    }
}
