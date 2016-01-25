using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Entity
{
    public abstract class EntityBase
    {
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}
