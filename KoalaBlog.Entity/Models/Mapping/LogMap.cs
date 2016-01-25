using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class LogMap : EntityTypeConfigurationBase<Log>
    {
        public LogMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.LogSource)
                .IsRequired()
                .HasMaxLength(350);

            this.Property(t => t.LogMessage)
                .IsRequired();

            this.Property(t => t.LogLevel)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Log");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.LogTime).HasColumnName("LogTime");
            this.Property(t => t.LogSource).HasColumnName("LogSource");
            this.Property(t => t.LogMessage).HasColumnName("LogMessage");
            this.Property(t => t.LogLevel).HasColumnName("LogLevel");
        }
    }
}
