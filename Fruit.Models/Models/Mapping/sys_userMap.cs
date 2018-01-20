using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_userMap : EntityTypeConfiguration<sys_user>
    {
        public sys_userMap()
        {
            // Primary Key
            this.HasKey(t => t.UserCode);

            // Properties
            this.Property(t => t.UserCode)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.UserSeq)
                .HasMaxLength(10);

            this.Property(t => t.UserName)
                .HasMaxLength(200);

            this.Property(t => t.Description)
                .HasMaxLength(2048);

            this.Property(t => t.Password)
                .HasMaxLength(40);

            this.Property(t => t.RoleName)
                .HasMaxLength(1000);

            this.Property(t => t.OrganizeName)
                .HasMaxLength(1000);

            this.Property(t => t.ConfigJSON)
                .HasMaxLength(4000);

            this.Property(t => t.CreatePerson)
                .HasMaxLength(20);

            this.Property(t => t.UpdatePerson)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("sys_user");
            this.Property(t => t.UserCode).HasColumnName("UserCode");
            this.Property(t => t.UserSeq).HasColumnName("UserSeq");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.RoleName).HasColumnName("RoleName");
            this.Property(t => t.OrganizeName).HasColumnName("OrganizeName");
            this.Property(t => t.ConfigJSON).HasColumnName("ConfigJSON");
            this.Property(t => t.IsEnable).HasColumnName("IsEnable");
            this.Property(t => t.LoginCount).HasColumnName("LoginCount");
            this.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");
            this.Property(t => t.CreatePerson).HasColumnName("CreatePerson");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdatePerson).HasColumnName("UpdatePerson");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
        }
    }
}
