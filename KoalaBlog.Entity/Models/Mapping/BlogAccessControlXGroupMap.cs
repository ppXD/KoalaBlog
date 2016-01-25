using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class BlogAccessControlXGroupMap : EntityTypeConfigurationBase<BlogAccessControlXGroup>
    {
        public BlogAccessControlXGroupMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("BlogAccessControlXGroup");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.BlogAccessControlID).HasColumnName("BlogAccessControlID");
            this.Property(t => t.GroupID).HasColumnName("GroupID");

            // Relationships
            this.HasRequired(t => t.BlogAccessControl)
                .WithMany(t => t.BlogAccessControlXGroups)
                .HasForeignKey(d => d.BlogAccessControlID);
            this.HasRequired(t => t.Group)
                .WithMany(t => t.BlogAccessControlXGroups)
                .HasForeignKey(d => d.GroupID);

        }
    }
}
