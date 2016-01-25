using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class Job : EntityBase
    {
        public long ID { get; set; }
        public long PersonID { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentOrPosition { get; set; }
        public Nullable<System.DateTime> WorkStartDate { get; set; }
        public System.DateTime WorkEndDate { get; set; }
        public string WorkCity { get; set; }
        public string WorkProvince { get; set; }
        public int AccessLevel { get; set; }
        public virtual Person Person { get; set; }
    }
}
