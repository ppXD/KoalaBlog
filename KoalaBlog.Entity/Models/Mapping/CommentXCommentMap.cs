using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class CommentXCommentMap : EntityTypeConfigurationBase<CommentXComment>
    {
        public CommentXCommentMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("CommentXComment");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.BaseCommentID).HasColumnName("BaseCommentID");
            this.Property(t => t.NewCommentID).HasColumnName("NewCommentID");

            // Relationships
            this.HasRequired(t => t.BaseComment)
                .WithMany(t => t.BaseCommentXComments)
                .HasForeignKey(d => d.BaseCommentID);
            this.HasRequired(t => t.NewComment)
                .WithMany(t => t.NewCommentXComments)
                .HasForeignKey(d => d.NewCommentID);

        }
    }
}
