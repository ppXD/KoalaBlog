using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace KoalaBlog.Entity.Models.Mapping
{
    public class PersonMap : EntityTypeConfigurationBase<Person>
    {
        public PersonMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.RealName)
                .HasMaxLength(20);

            this.Property(t => t.NickName)
                .HasMaxLength(50);

            this.Property(t => t.QQ)
                .HasMaxLength(20);

            this.Property(t => t.HomePage)
                .HasMaxLength(50);

            this.Property(t => t.Introduction)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Person");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastModifiedDate).HasColumnName("LastModifiedDate");
            this.Property(t => t.LastModifiedBy).HasColumnName("LastModifiedBy");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.NickName).HasColumnName("NickName");
            this.Property(t => t.Gender).HasColumnName("Gender");
            this.Property(t => t.SexualTrend).HasColumnName("SexualTrend");
            this.Property(t => t.MaritalStatus).HasColumnName("MaritalStatus");
            this.Property(t => t.QQ).HasColumnName("QQ");
            this.Property(t => t.DOB).HasColumnName("DOB");
            this.Property(t => t.HomePage).HasColumnName("HomePage");
            this.Property(t => t.BloodType).HasColumnName("BloodType");
            this.Property(t => t.Introduction).HasColumnName("Introduction");
            this.Property(t => t.RealNameAccessLevel).HasColumnName("RealNameAccessLevel");
            this.Property(t => t.SexualTrendAccessLevel).HasColumnName("SexualTrendAccessLevel");
            this.Property(t => t.MaritalStatusAccessLevel).HasColumnName("MaritalStatusAccessLevel");
            this.Property(t => t.QQAccessLevel).HasColumnName("QQAccessLevel");
            this.Property(t => t.DOBAccessLevel).HasColumnName("DOBAccessLevel");
            this.Property(t => t.BloodTypeAccessLevel).HasColumnName("BloodTypeAccessLevel");
            this.Property(t => t.HomePageAccessLevel).HasColumnName("HomePageAccessLevel");
            this.Property(t => t.AllowablePersonForComment).HasColumnName("AllowablePersonForComment");
            this.Property(t => t.AllowCommentAttachContent).HasColumnName("AllowCommentAttachContent");
        }
    }
}
