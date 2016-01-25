using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class EntityComplain : EntityBase
    {
        public long ID { get; set; }
        public long PersonID { get; set; }
        public long EntityID { get; set; }
        public string EntityTableName { get; set; }
        public int Type { get; set; }
        public string Reason { get; set; }
        public virtual Person Person { get; set; }
    }
}
