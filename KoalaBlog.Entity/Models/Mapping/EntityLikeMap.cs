using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class EntityLikeMap : EntityTypeConfigurationBase<EntityLike>
    {
        public EntityLikeMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.EntityTableName)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("EntityLike");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.EntityID).HasColumnName("EntityID");
            this.Property(t => t.EntityTableName).HasColumnName("EntityTableName");

            // Relationships
            this.HasRequired(t => t.Person)
                .WithMany(t => t.EntityLikes)
                .HasForeignKey(d => d.PersonID);

        }
    }
}
