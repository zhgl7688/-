using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_roleMenuColumnMapMap : EntityTypeConfiguration<sys_roleMenuColumnMap>
    {
        public sys_roleMenuColumnMapMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.RoleCode)
                .HasMaxLength(100);

            this.Property(t => t.MenuCode)
                .HasMaxLength(100);

            this.Property(t => t.FieldName)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("sys_roleMenuColumnMap");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.RoleCode).HasColumnName("RoleCode");
            this.Property(t => t.MenuCode).HasColumnName("MenuCode");
            this.Property(t => t.IsReject).HasColumnName("IsReject");
            this.Property(t => t.FieldName).HasColumnName("FieldName");

            // Relationships
            this.HasOptional(t => t.sys_menu)
                .WithMany(t => t.sys_roleMenuColumnMap)
                .HasForeignKey(d => d.MenuCode);
            this.HasOptional(t => t.sys_role)
                .WithMany(t => t.sys_roleMenuColumnMap)
                .HasForeignKey(d => d.RoleCode);

        }
    }
}
