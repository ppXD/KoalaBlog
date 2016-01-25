using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class RoleXUserAccount : EntityBase
    {
        public long ID { get; set; }
        public long UserAccountID { get; set; }
        public long RoleID { get; set; }
        public virtual Role Role { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
