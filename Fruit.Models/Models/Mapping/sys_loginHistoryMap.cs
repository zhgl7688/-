using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_loginHistoryMap : EntityTypeConfiguration<sys_loginHistory>
    {
        public sys_loginHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.UserCode)
                .HasMaxLength(20);

            this.Property(t => t.UserName)
                .HasMaxLength(200);

            this.Property(t => t.HostName)
                .HasMaxLength(200);

            this.Property(t => t.HostIP)
                .HasMaxLength(50);

            this.Property(t => t.LoginCity)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("sys_loginHistory");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.UserCode).HasColumnName("UserCode");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.HostName).HasColumnName("HostName");
            this.Property(t => t.HostIP).HasColumnName("HostIP");
            this.Property(t => t.LoginCity).HasColumnName("LoginCity");
            this.Property(t => t.LoginDate).HasColumnName("LoginDate");
        }
    }
}
