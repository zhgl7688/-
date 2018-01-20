using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_organizeMap : EntityTypeConfiguration<sys_organize>
    {
        public sys_organizeMap()
        {
            // Primary Key
            this.HasKey(t => t.OrganizeCode);

            // Properties
            this.Property(t => t.OrganizeCode)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ParentCode)
                .HasMaxLength(100);

            this.Property(t => t.OrganizeSeq)
                .HasMaxLength(10);

            this.Property(t => t.OrganizeName)
                .HasMaxLength(200);

            this.Property(t => t.Description)
                .HasMaxLength(2048);

            this.Property(t => t.CreatePerson)
                .HasMaxLength(20);

            this.Property(t => t.UpdatePerson)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("sys_organize");
            this.Property(t => t.OrganizeCode).HasColumnName("OrganizeCode");
            this.Property(t => t.ParentCode).HasColumnName("ParentCode");
            this.Property(t => t.OrganizeSeq).HasColumnName("OrganizeSeq");
            this.Property(t => t.OrganizeName).HasColumnName("OrganizeName");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.CreatePerson).HasColumnName("CreatePerson");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdatePerson).HasColumnName("UpdatePerson");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
        }
    }
}
