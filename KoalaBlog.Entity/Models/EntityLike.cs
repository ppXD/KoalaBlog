using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class EntityLike : EntityBase
    {
        public long ID { get; set; }
        public long PersonID { get; set; }
        public long EntityID { get; set; }
        public string EntityTableName { get; set; }
        public virtual Person Person { get; set; }
    }
}
