using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class FavoriteMap : EntityTypeConfigurationBase<Favorite>
    {
        public FavoriteMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Favorite");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.BlogID).HasColumnName("BlogID");

            // Relationships
            this.HasRequired(t => t.Blog)
                .WithMany(t => t.Favorites)
                .HasForeignKey(d => d.BlogID);
            this.HasRequired(t => t.Person)
                .WithMany(t => t.Favorites)
                .HasForeignKey(d => d.PersonID);

        }
    }
}
