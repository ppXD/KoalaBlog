using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class EducationMap : EntityTypeConfigurationBase<Education>
    {
        public EducationMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Education");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.SchoolID).HasColumnName("SchoolID");
            this.Property(t => t.SchoolYear).HasColumnName("SchoolYear");
            this.Property(t => t.AccessLevel).HasColumnName("AccessLevel");

            // Relationships
            this.HasRequired(t => t.Person)
                .WithMany(t => t.Educations)
                .HasForeignKey(d => d.PersonID);
            this.HasRequired(t => t.School)
                .WithMany(t => t.Educations)
                .HasForeignKey(d => d.SchoolID);

        }
    }
}
