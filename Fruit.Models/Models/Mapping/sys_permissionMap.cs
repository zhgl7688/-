using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_permissionMap : EntityTypeConfiguration<sys_permission>
    {
        public sys_permissionMap()
        {
            // Primary Key
            this.HasKey(t => t.PermissionCode);

            // Properties
            this.Property(t => t.PermissionCode)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.PermissionName)
                .HasMaxLength(200);

            this.Property(t => t.ParentCode)
                .HasMaxLength(100);

            this.Property(t => t.CreatePerson)
                .HasMaxLength(20);

            this.Property(t => t.UpdatePerson)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("sys_permission");
            this.Property(t => t.PermissionCode).HasColumnName("PermissionCode");
            this.Property(t => t.PermissionName).HasColumnName("PermissionName");
            this.Property(t => t.ParentCode).HasColumnName("ParentCode");
            this.Property(t => t.CreatePerson).HasColumnName("CreatePerson");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdatePerson).HasColumnName("UpdatePerson");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
        }
    }
}
