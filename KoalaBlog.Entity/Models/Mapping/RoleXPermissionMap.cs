using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class RoleXPermissionMap : EntityTypeConfigurationBase<RoleXPermission>
    {
        public RoleXPermissionMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("RoleXPermission");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.PermissionID).HasColumnName("PermissionID");

            // Relationships
            this.HasRequired(t => t.Permission)
                .WithMany(t => t.RoleXPermissions)
                .HasForeignKey(d => d.PermissionID);
            this.HasRequired(t => t.Role)
                .WithMany(t => t.RoleXPermissions)
                .HasForeignKey(d => d.RoleID);

        }
    }
}
