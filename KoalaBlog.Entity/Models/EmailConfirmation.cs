using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class EmailConfirmation : EntityBase
    {
        public long ID { get; set; }
        public long UserAccountID { get; set; }
        public string Code { get; set; }
        public KoalaBlog.Entity.Models.Enums.EmailConfirmationType Type { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
