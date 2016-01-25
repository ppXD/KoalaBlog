using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class SchoolMap : EntityTypeConfigurationBase<School>
    {
        public SchoolMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Class)
                .HasMaxLength(30);

            this.Property(t => t.Department)
                .HasMaxLength(30);

            this.Property(t => t.Location)
                .IsRequired()
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("School");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.SchoolCategoryID).HasColumnName("SchoolCategoryID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Class).HasColumnName("Class");
            this.Property(t => t.Department).HasColumnName("Department");
            this.Property(t => t.Location).HasColumnName("Location");

            // Relationships
            this.HasRequired(t => t.SchoolCategory)
                .WithMany(t => t.Schools)
                .HasForeignKey(d => d.SchoolCategoryID);

        }
    }
}
