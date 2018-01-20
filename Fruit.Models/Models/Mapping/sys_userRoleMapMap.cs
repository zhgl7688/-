using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_userRoleMapMap : EntityTypeConfiguration<sys_userRoleMap>
    {
        public sys_userRoleMapMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.UserCode)
                .HasMaxLength(100);

            this.Property(t => t.RoleCode)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("sys_userRoleMap");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.UserCode).HasColumnName("UserCode");
            this.Property(t => t.RoleCode).HasColumnName("RoleCode");

            // Relationships
            this.HasOptional(t => t.sys_role)
                .WithMany(t => t.sys_userRoleMap)
                .HasForeignKey(d => d.RoleCode);
            this.HasOptional(t => t.sys_user)
                .WithMany(t => t.sys_userRoleMap)
                .HasForeignKey(d => d.UserCode);

        }
    }
}
