using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_logMap : EntityTypeConfiguration<sys_log>
    {
        public sys_logMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.UserCode)
                .HasMaxLength(20);

            this.Property(t => t.UserName)
                .HasMaxLength(100);

            this.Property(t => t.Position)
                .HasMaxLength(1024);

            this.Property(t => t.Target)
                .HasMaxLength(255);

            this.Property(t => t.Type)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("sys_log");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.UserCode).HasColumnName("UserCode");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Position).HasColumnName("Position");
            this.Property(t => t.Target).HasColumnName("Target");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.Date).HasColumnName("Date");
        }
    }
}
