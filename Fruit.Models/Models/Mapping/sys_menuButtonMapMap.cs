using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Fruit.Models.Mapping
{
    public class sys_menuButtonMapMap : EntityTypeConfiguration<sys_menuButtonMap>
    {
        public sys_menuButtonMapMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.MenuCode)
                .HasMaxLength(100);

            this.Property(t => t.ButtonCode)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("sys_menuButtonMap");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.MenuCode).HasColumnName("MenuCode");
            this.Property(t => t.ButtonCode).HasColumnName("ButtonCode");

            // Relationships
            this.HasOptional(t => t.sys_button)
                .WithMany(t => t.sys_menuButtonMap)
                .HasForeignKey(d => d.ButtonCode);
            this.HasOptional(t => t.sys_menu)
                .WithMany(t => t.sys_menuButtonMap)
                .HasForeignKey(d => d.MenuCode);
            this.HasOptional(t => t.sys_menu1)
                .WithMany(t => t.sys_menuButtonMap1)
                .HasForeignKey(d => d.MenuCode);

        }
    }
}
