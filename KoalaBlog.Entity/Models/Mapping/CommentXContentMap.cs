using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class CommentXContentMap : EntityTypeConfigurationBase<CommentXContent>
    {
        public CommentXContentMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("CommentXContent");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.CommentID).HasColumnName("CommentID");
            this.Property(t => t.ContentID).HasColumnName("ContentID");

            // Relationships
            this.HasRequired(t => t.Comment)
                .WithMany(t => t.CommentXContents)
                .HasForeignKey(d => d.CommentID);
            this.HasRequired(t => t.Content)
                .WithMany(t => t.CommentXContents)
                .HasForeignKey(d => d.ContentID);

        }
    }
}
