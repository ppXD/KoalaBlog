using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class UserAccountXPersonMap : EntityTypeConfigurationBase<UserAccountXPerson>
    {
        public UserAccountXPersonMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("UserAccountXPerson");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.UserAccountID).HasColumnName("UserAccountID");
            this.Property(t => t.PersonID).HasColumnName("PersonID");

            // Relationships
            this.HasRequired(t => t.Person)
                .WithMany(t => t.UserAccountXPersons)
                .HasForeignKey(d => d.PersonID);
            this.HasRequired(t => t.UserAccount)
                .WithMany(t => t.UserAccountXPersons)
                .HasForeignKey(d => d.UserAccountID);

        }
    }
}
