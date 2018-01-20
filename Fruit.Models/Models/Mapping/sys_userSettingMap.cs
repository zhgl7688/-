using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_userSettingMap : EntityTypeConfiguration<sys_userSetting>
    {
        public sys_userSettingMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.UserCode)
                .HasMaxLength(100);

            this.Property(t => t.SettingCode)
                .HasMaxLength(100);

            this.Property(t => t.SettingName)
                .HasMaxLength(200);

            this.Property(t => t.SettingValue)
                .HasMaxLength(100);

            this.Property(t => t.Description)
                .HasMaxLength(2048);

            // Table & Column Mappings
            this.ToTable("sys_userSetting");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.UserCode).HasColumnName("UserCode");
            this.Property(t => t.SettingCode).HasColumnName("SettingCode");
            this.Property(t => t.SettingName).HasColumnName("SettingName");
            this.Property(t => t.SettingValue).HasColumnName("SettingValue");
            this.Property(t => t.Description).HasColumnName("Description");

            // Relationships
            this.HasOptional(t => t.sys_user)
                .WithMany(t => t.sys_userSetting)
                .HasForeignKey(d => d.UserCode);

        }
    }
}
