using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class AddressMap : EntityTypeConfigurationBase<Address>
    {
        public AddressMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.AddressLine1)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.AddressLine2)
                .HasMaxLength(150);

            this.Property(t => t.City)
                .HasMaxLength(20);

            this.Property(t => t.Province)
                .HasMaxLength(20);

            this.Property(t => t.Country)
                .HasMaxLength(20);

            this.Property(t => t.PostCode)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Address");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.PersonID).HasColumnName("PersonID");
            this.Property(t => t.AddressLine1).HasColumnName("AddressLine1");
            this.Property(t => t.AddressLine2).HasColumnName("AddressLine2");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Province).HasColumnName("Province");
            this.Property(t => t.Country).HasColumnName("Country");
            this.Property(t => t.PostCode).HasColumnName("PostCode");
            this.Property(t => t.AddressType).HasColumnName("AddressType");

            // Relationships
            this.HasRequired(t => t.Person)
                .WithMany(t => t.Addresses)
                .HasForeignKey(d => d.PersonID);

        }
    }
}
