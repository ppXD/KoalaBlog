using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class PersonXPerson : EntityBase
    {
        public long ID { get; set; }
        /// <summary>
        /// ¹Ø×¢ID
        /// </summary>
        public long FollowerID { get; set; }
        /// <summary>
        /// ±»¹Ø×¢ID
        /// </summary>
        public long FollowingID { get; set; }
        public virtual Person Follower { get; set; }
        public virtual Person Following { get; set; }
    }
}
