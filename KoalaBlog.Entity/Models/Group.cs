using KoalaBlog.Entity.Models.Enums;
using System;
using System.Collections.Generic;

namespace KoalaBlog.Entity.Models
{
    public partial class Group : EntityBase
    {
        public Group()
        {
            this.BlogAccessControlXGroups = new List<BlogAccessControlXGroup>();
            this.GroupMembers = new List<GroupMember>();
        }

        public long ID { get; set; }
        public long PersonID { get; set; }
        public string Name { get; set; }
        public GroupType Type { get; set; }
        public virtual ICollection<BlogAccessControlXGroup> BlogAccessControlXGroups { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<GroupMember> GroupMembers { get; set; }
    }
}
