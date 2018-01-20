using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_roleMenuButtonMapMap : EntityTypeConfiguration<sys_roleMenuButtonMap>
    {
        public sys_roleMenuButtonMapMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.RoleCode)
                .HasMaxLength(100);

            this.Property(t => t.MenuCode)
                .HasMaxLength(100);

            this.Property(t => t.ButtonCode)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("sys_roleMenuButtonMap");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.RoleCode).HasColumnName("RoleCode");
            this.Property(t => t.MenuCode).HasColumnName("MenuCode");
            this.Property(t => t.ButtonCode).HasColumnName("ButtonCode");

            // Relationships
            this.HasOptional(t => t.sys_button)
                .WithMany(t => t.sys_roleMenuButtonMap)
                .HasForeignKey(d => d.ButtonCode);
            this.HasOptional(t => t.sys_menu)
                .WithMany(t => t.sys_roleMenuButtonMap)
                .HasForeignKey(d => d.MenuCode);
            this.HasOptional(t => t.sys_role)
                .WithMany(t => t.sys_roleMenuButtonMap)
                .HasForeignKey(d => d.RoleCode);

        }
    }
}
