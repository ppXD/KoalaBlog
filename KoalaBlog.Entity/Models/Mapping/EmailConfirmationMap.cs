using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class EmailConfirmationMap : EntityTypeConfigurationBase<EmailConfirmation>
    {
        public EmailConfirmationMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.Type)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("EmailConfirmation");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.UserAccountID).HasColumnName("UserAccountID");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Type).HasColumnName("Type");

            // Relationships
            this.HasRequired(t => t.UserAccount)
                .WithMany(t => t.EmailConfirmations)
                .HasForeignKey(d => d.UserAccountID);

        }
    }
}
