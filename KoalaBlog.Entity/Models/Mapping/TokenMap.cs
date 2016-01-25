using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class TokenMap : EntityTypeConfigurationBase<Token>
    {
        public TokenMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.AccessToken)
                .IsRequired()
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Token");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.UserAccountID).HasColumnName("UserAccountID");
            this.Property(t => t.AccessToken).HasColumnName("AccessToken");
            this.Property(t => t.TokenType).HasColumnName("TokenType");
            this.Property(t => t.ExpirationDate).HasColumnName("ExpirationDate");
            this.Property(t => t.IsSlidingExpiration).HasColumnName("IsSlidingExpiration");
            this.Property(t => t.IsRevoked).HasColumnName("IsRevoked");

            // Relationships
            this.HasRequired(t => t.UserAccount)
                .WithMany(t => t.Tokens)
                .HasForeignKey(d => d.UserAccountID);

        }
    }
}
