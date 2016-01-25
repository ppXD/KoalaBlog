using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class Address : EntityBase
    {
        public long ID { get; set; }
        public long PersonID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public int AddressType { get; set; }
        public virtual Person Person { get; set; }
    }
}
