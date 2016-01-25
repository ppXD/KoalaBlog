using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class UserAccountXPerson : EntityBase
    {
        public long ID { get; set; }
        public long UserAccountID { get; set; }
        public long PersonID { get; set; }
        public virtual Person Person { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
