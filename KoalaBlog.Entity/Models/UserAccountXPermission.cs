using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class UserAccountXPermission : EntityBase
    {
        public long ID { get; set; }
        public long UserAccountID { get; set; }
        public long PermissionID { get; set; }
        public virtual Permission Permission { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
