using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaBlog.Entity.Models
{
    public partial class EntityConcurrentLock
    {
        public long PersonID { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
