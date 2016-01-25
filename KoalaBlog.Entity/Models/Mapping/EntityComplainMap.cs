using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class EntityComplainMap : EntityTypeConfigurationBase<EntityComplain>
    {
        public EntityComplainMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.EntityTableName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Reason)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("EntityComplain");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.EntityID).HasColumnName("EntityID");
            this.Property(t => t.EntityTableName).HasColumnName("EntityTableName");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Reason).HasColumnName("Reason");

            // Relationships
            this.HasRequired(t => t.Person)
                .WithMany(t => t.EntityComplains)
                .HasForeignKey(d => d.PersonID);

        }
    }
}
