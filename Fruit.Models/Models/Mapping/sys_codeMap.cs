using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_codeMap : EntityTypeConfiguration<sys_code>
    {
        public sys_codeMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Value)
                .HasMaxLength(200);

            this.Property(t => t.Text)
                .HasMaxLength(200);

            this.Property(t => t.ParentCode)
                .HasMaxLength(100);

            this.Property(t => t.Seq)
                .HasMaxLength(10);

            this.Property(t => t.Description)
                .HasMaxLength(2048);

            this.Property(t => t.CodeTypeName)
                .HasMaxLength(200);

            this.Property(t => t.CodeType)
                .HasMaxLength(100);

            this.Property(t => t.CreatePerson)
                .HasMaxLength(20);

            this.Property(t => t.UpdatePerson)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("sys_code");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.Text).HasColumnName("Text");
            this.Property(t => t.ParentCode).HasColumnName("ParentCode");
            this.Property(t => t.Seq).HasColumnName("Seq");
            this.Property(t => t.IsEnable).HasColumnName("IsEnable");
            this.Property(t => t.IsDefault).HasColumnName("IsDefault");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.CodeTypeName).HasColumnName("CodeTypeName");
            this.Property(t => t.CodeType).HasColumnName("CodeType");
            this.Property(t => t.CreatePerson).HasColumnName("CreatePerson");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdatePerson).HasColumnName("UpdatePerson");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");

            // Relationships
            this.HasOptional(t => t.sys_codeType)
                .WithMany(t => t.sys_code)
                .HasForeignKey(d => d.CodeType);

        }
    }
}
