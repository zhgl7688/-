using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_menuMap : EntityTypeConfiguration<sys_menu>
    {
        public sys_menuMap()
        {
            // Primary Key
            this.HasKey(t => t.MenuCode);

            // Properties
            this.Property(t => t.MenuCode)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ParentCode)
                .HasMaxLength(100);

            this.Property(t => t.MenuName)
                .HasMaxLength(200);

            this.Property(t => t.URL)
                .HasMaxLength(200);

            this.Property(t => t.IconClass)
                .HasMaxLength(50);

            this.Property(t => t.IconURL)
                .HasMaxLength(200);

            this.Property(t => t.MenuSeq)
                .HasMaxLength(10);

            this.Property(t => t.Description)
                .HasMaxLength(2048);

            this.Property(t => t.CreatePerson)
                .HasMaxLength(20);

            this.Property(t => t.UpdatePerson)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("sys_menu");
            this.Property(t => t.MenuCode).HasColumnName("MenuCode");
            this.Property(t => t.ParentCode).HasColumnName("ParentCode");
            this.Property(t => t.MenuName).HasColumnName("MenuName");
            this.Property(t => t.URL).HasColumnName("URL");
            this.Property(t => t.IconClass).HasColumnName("IconClass");
            this.Property(t => t.IconURL).HasColumnName("IconURL");
            this.Property(t => t.MenuSeq).HasColumnName("MenuSeq");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.IsVisible).HasColumnName("IsVisible");
            this.Property(t => t.IsEnable).HasColumnName("IsEnable");
            this.Property(t => t.CreatePerson).HasColumnName("CreatePerson");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdatePerson).HasColumnName("UpdatePerson");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
        }
    }
}
