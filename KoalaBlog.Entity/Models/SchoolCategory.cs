using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class SchoolCategory : EntityBase
    {
        public SchoolCategory()
        {
            this.Schools = new List<School>();
        }

        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<School> Schools { get; set; }
    }
}
