using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class UserAccountXPermissionMap : EntityTypeConfigurationBase<UserAccountXPermission>
    {
        public UserAccountXPermissionMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("UserAccountXPermission");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.UserAccountID).HasColumnName("UserAccountID");
            this.Property(t => t.PermissionID).HasColumnName("PermissionID");

            // Relationships
            this.HasRequired(t => t.Permission)
                .WithMany(t => t.UserAccountXPermissions)
                .HasForeignKey(d => d.PermissionID);
            this.HasRequired(t => t.UserAccount)
                .WithMany(t => t.UserAccountXPermissions)
                .HasForeignKey(d => d.UserAccountID);

        }
    }
}
