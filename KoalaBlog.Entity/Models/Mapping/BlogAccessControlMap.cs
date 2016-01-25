using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class BlogAccessControlMap : EntityTypeConfigurationBase<BlogAccessControl>
    {
        public BlogAccessControlMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("BlogAccessControl");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.BlogID).HasColumnName("BlogID");
            this.Property(t => t.AccessLevel).HasColumnName("AccessLevel");

            // Relationships
            this.HasRequired(t => t.Blog)
                .WithMany(t => t.BlogAccessControls)
                .HasForeignKey(d => d.BlogID);
        }
    }
}
