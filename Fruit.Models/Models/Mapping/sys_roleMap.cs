using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_roleMap : EntityTypeConfiguration<sys_role>
    {
        public sys_roleMap()
        {
            // Primary Key
            this.HasKey(t => t.RoleCode);

            // Properties
            this.Property(t => t.RoleCode)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.RoleSeq)
                .HasMaxLength(10);

            this.Property(t => t.RoleName)
                .HasMaxLength(200);

            this.Property(t => t.Description)
                .HasMaxLength(2048);

            this.Property(t => t.CreatePerson)
                .HasMaxLength(20);

            this.Property(t => t.UpdatePerson)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("sys_role");
            this.Property(t => t.RoleCode).HasColumnName("RoleCode");
            this.Property(t => t.RoleSeq).HasColumnName("RoleSeq");
            this.Property(t => t.RoleName).HasColumnName("RoleName");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.CreatePerson).HasColumnName("CreatePerson");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdatePerson).HasColumnName("UpdatePerson");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
        }
    }
}
