using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class EntityConcurrentLockMap : EntityTypeConfigurationBase<EntityConcurrentLock>
    {
        public EntityConcurrentLockMap()
        {
            // Primary Key
            this.HasKey(t => t.PersonID);

            // Properties
            this.Property(t => t.PersonID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RowVersion)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("EntityConcurrentLock");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");
        }
    }
}
