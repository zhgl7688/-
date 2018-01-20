using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_rolePermissionMapMap : EntityTypeConfiguration<sys_rolePermissionMap>
    {
        public sys_rolePermissionMapMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.RoleCode)
                .HasMaxLength(100);

            this.Property(t => t.PermissionCode)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("sys_rolePermissionMap");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.RoleCode).HasColumnName("RoleCode");
            this.Property(t => t.PermissionCode).HasColumnName("PermissionCode");
            this.Property(t => t.IsDefault).HasColumnName("IsDefault");

            // Relationships
            this.HasOptional(t => t.sys_permission)
                .WithMany(t => t.sys_rolePermissionMap)
                .HasForeignKey(d => d.PermissionCode);
            this.HasOptional(t => t.sys_role)
                .WithMany(t => t.sys_rolePermissionMap)
                .HasForeignKey(d => d.RoleCode);

        }
    }
}
