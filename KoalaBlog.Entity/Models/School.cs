using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class School : EntityBase
    {
        public School()
        {
            this.Educations = new List<Education>();
        }

        public long ID { get; set; }
        public long SchoolCategoryID { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public virtual SchoolCategory SchoolCategory { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
    }
}
