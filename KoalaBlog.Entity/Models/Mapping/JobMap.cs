using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class JobMap : EntityTypeConfigurationBase<Job>
    {
        public JobMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.CompanyName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DepartmentOrPosition)
                .HasMaxLength(50);

            this.Property(t => t.WorkCity)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.WorkProvince)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Job");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
            this.Property(t => t.DepartmentOrPosition).HasColumnName("DepartmentOrPosition");
            this.Property(t => t.WorkStartDate).HasColumnName("WorkStartDate");
            this.Property(t => t.WorkEndDate).HasColumnName("WorkEndDate");
            this.Property(t => t.WorkCity).HasColumnName("WorkCity");
            this.Property(t => t.WorkProvince).HasColumnName("WorkProvince");
            this.Property(t => t.AccessLevel).HasColumnName("AccessLevel");

            // Relationships
            this.HasRequired(t => t.Person)
                .WithMany(t => t.Jobs)
                .HasForeignKey(d => d.PersonID);

        }
    }
}
