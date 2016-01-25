using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Entity.Models
{
    public class Education : EntityBase
    {
        public long ID { get; set; }
        public long PersonID { get; set; }
        public long SchoolID { get; set; }
        public int SchoolYear { get; set; }
        public int AccessLevel { get; set; }
        public virtual Person Person { get; set; }
        public virtual School School { get; set; }
    }
}
