using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_userOrganizeMapMap : EntityTypeConfiguration<sys_userOrganizeMap>
    {
        public sys_userOrganizeMapMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.OrganizeCode)
                .HasMaxLength(100);

            this.Property(t => t.UserCode)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("sys_userOrganizeMap");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.OrganizeCode).HasColumnName("OrganizeCode");
            this.Property(t => t.UserCode).HasColumnName("UserCode");

            // Relationships
            this.HasOptional(t => t.sys_organize)
                .WithMany(t => t.sys_userOrganizeMap)
                .HasForeignKey(d => d.OrganizeCode);
            this.HasOptional(t => t.sys_user)
                .WithMany(t => t.sys_userOrganizeMap)
                .HasForeignKey(d => d.UserCode);

        }
    }
}
