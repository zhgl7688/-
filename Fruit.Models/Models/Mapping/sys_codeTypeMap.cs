using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_codeTypeMap : EntityTypeConfiguration<sys_codeType>
    {
        public sys_codeTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.CodeType);

            // Properties
            this.Property(t => t.CodeType)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CodeTypeName)
                .HasMaxLength(200);

            this.Property(t => t.Description)
                .HasMaxLength(2048);

            this.Property(t => t.Seq)
                .HasMaxLength(10);

            this.Property(t => t.CreatePerson)
                .HasMaxLength(20);

            this.Property(t => t.UpdatePerson)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("sys_codeType");
            this.Property(t => t.CodeType).HasColumnName("CodeType");
            this.Property(t => t.CodeTypeName).HasColumnName("CodeTypeName");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Seq).HasColumnName("Seq");
            this.Property(t => t.CreatePerson).HasColumnName("CreatePerson");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdatePerson).HasColumnName("UpdatePerson");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
        }
    }
}
