using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_parameterMap : EntityTypeConfiguration<sys_parameter>
    {
        public sys_parameterMap()
        {
            // Primary Key
            this.HasKey(t => t.ParamCode);

            // Properties
            this.Property(t => t.ParamCode)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ParamValue)
                .HasMaxLength(200);

            this.Property(t => t.Description)
                .HasMaxLength(2048);

            this.Property(t => t.CreatePerson)
                .HasMaxLength(20);

            this.Property(t => t.UpdatePerson)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("sys_parameter");
            this.Property(t => t.ParamCode).HasColumnName("ParamCode");
            this.Property(t => t.ParamValue).HasColumnName("ParamValue");
            this.Property(t => t.IsUserEditable).HasColumnName("IsUserEditable");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.CreatePerson).HasColumnName("CreatePerson");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdatePerson).HasColumnName("UpdatePerson");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
        }
    }
}
