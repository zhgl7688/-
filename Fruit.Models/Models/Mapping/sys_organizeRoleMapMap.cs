using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_organizeRoleMapMap : EntityTypeConfiguration<sys_organizeRoleMap>
    {
        public sys_organizeRoleMapMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.OrganizeCode)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.RoleCode)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("sys_organizeRoleMap");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.OrganizeCode).HasColumnName("OrganizeCode");
            this.Property(t => t.RoleCode).HasColumnName("RoleCode");

            // Relationships
            this.HasRequired(t => t.sys_organize)
                .WithMany(t => t.sys_organizeRoleMap)
                .HasForeignKey(d => d.OrganizeCode);
            this.HasRequired(t => t.sys_role)
                .WithMany(t => t.sys_organizeRoleMap)
                .HasForeignKey(d => d.RoleCode);

        }
    }
}
