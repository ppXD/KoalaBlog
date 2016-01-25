using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class ContentMap : EntityTypeConfigurationBase<Content>
    {
        public ContentMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ContentPath)
                .HasMaxLength(150);

            this.Property(t => t.MimeType)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.AltAttribute)
                .HasMaxLength(30);

            this.Property(t => t.TitleAttribute)
                .HasMaxLength(50);

            this.Property(t => t.SeoFilename)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("Content");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.ContentPath).HasColumnName("ContentPath");
            this.Property(t => t.ContentBinary).HasColumnName("ContentBinary");
            this.Property(t => t.MimeType).HasColumnName("MimeType");
            this.Property(t => t.AltAttribute).HasColumnName("AltAttribute");
            this.Property(t => t.TitleAttribute).HasColumnName("TitleAttribute");
            this.Property(t => t.SeoFilename).HasColumnName("SeoFilename");
            this.Property(t => t.Type).HasColumnName("Type");
        }
    }
}
