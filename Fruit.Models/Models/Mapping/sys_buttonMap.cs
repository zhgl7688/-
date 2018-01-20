using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_buttonMap : EntityTypeConfiguration<sys_button>
    {
        public sys_buttonMap()
        {
            // Primary Key
            this.HasKey(t => t.ButtonCode);

            // Properties
            this.Property(t => t.ButtonCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ButtonName)
                .HasMaxLength(20);

            this.Property(t => t.Description)
                .HasMaxLength(100);

            this.Property(t => t.ButtonIcon)
                .HasMaxLength(50);

            this.Property(t => t.CreatePerson)
                .HasMaxLength(20);

            this.Property(t => t.UpdatePerson)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("sys_button");
            this.Property(t => t.ButtonCode).HasColumnName("ButtonCode");
            this.Property(t => t.ButtonName).HasColumnName("ButtonName");
            this.Property(t => t.ButtonSeq).HasColumnName("ButtonSeq");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.ButtonIcon).HasColumnName("ButtonIcon");
            this.Property(t => t.CreatePerson).HasColumnName("CreatePerson");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdatePerson).HasColumnName("UpdatePerson");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
        }
    }
}
