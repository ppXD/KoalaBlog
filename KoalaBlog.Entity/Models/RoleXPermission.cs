using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class RoleXPermission : EntityBase
    {
        public long ID { get; set; }
        public long RoleID { get; set; }
        public long PermissionID { get; set; }
        public virtual Permission Permission { get; set; }
        public virtual Role Role { get; set; }
    }
}
