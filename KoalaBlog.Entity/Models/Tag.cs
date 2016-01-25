using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class Tag : EntityBase
    {
        public long ID { get; set; }
        public long PersonID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Person Person { get; set; }
    }
}
