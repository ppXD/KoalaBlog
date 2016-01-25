using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class Favorite : EntityBase
    {
        public long ID { get; set; }
        public long PersonID { get; set; }
        public long BlogID { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual Person Person { get; set; }
    }
}
