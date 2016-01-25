using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class GroupMemberMap : EntityTypeConfigurationBase<GroupMember>
    {
        public GroupMemberMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("GroupMember");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.GroupID).HasColumnName("GroupID");
            this.Property(t => t.PersonID).HasColumnName("PersonID");

            // Relationships
            this.HasRequired(t => t.Group)
                .WithMany(t => t.GroupMembers)
                .HasForeignKey(d => d.GroupID);
            this.HasRequired(t => t.Person)
                .WithMany(t => t.GroupMembers)
                .HasForeignKey(d => d.PersonID);

        }
    }
}
