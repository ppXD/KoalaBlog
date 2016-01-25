using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class PersonXPersonMap : EntityTypeConfigurationBase<PersonXPerson>
    {
        public PersonXPersonMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("PersonXPerson");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.FollowerID).HasColumnName("FollowerID");
            this.Property(t => t.FollowingID).HasColumnName("FollowingID");

            // Relationships
            this.HasRequired(t => t.Following)
                .WithMany(t => t.MyFans)
                .HasForeignKey(d => d.FollowingID);
            this.HasRequired(t => t.Follower)
                .WithMany(t => t.MyFollowingPersons)
                .HasForeignKey(d => d.FollowerID);

        }
    }
}
