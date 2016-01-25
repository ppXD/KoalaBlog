using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class BlogXBlogMap : EntityTypeConfigurationBase<BlogXBlog>
    {
        public BlogXBlogMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("BlogXBlog");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.BaseBlogID).HasColumnName("BaseBlogID");
            this.Property(t => t.NewBlogID).HasColumnName("NewBlogID");
            this.Property(t => t.IsBase).HasColumnName("IsBase");

            // Relationships
            this.HasRequired(t => t.BaseBlog)
                .WithMany(t => t.BaseBlogXBlogs)
                .HasForeignKey(d => d.BaseBlogID);
            this.HasRequired(t => t.NewBlog)
                .WithMany(t => t.NewBlogXBlogs)
                .HasForeignKey(d => d.NewBlogID);

        }
    }
}
