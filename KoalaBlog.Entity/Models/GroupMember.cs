using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class GroupMember : EntityBase
    {
        public long ID { get; set; }
        public long GroupID { get; set; }
        public long PersonID { get; set; }
        public virtual Group Group { get; set; }
        public virtual Person Person { get; set; }
    }
}
