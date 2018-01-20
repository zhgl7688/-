using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Fruit.Models.Mapping;

namespace Fruit.Models
{
    public partial class SysContext : DbContext
    {
        static SysContext()
        {
            Database.SetInitializer<SysContext>(null);
        }

        public SysContext()
            : base("Name=SysContext")
        {
        }

        public DbSet<sys_button> sys_button { get; set; }
        public DbSet<sys_code> sys_code { get; set; }
        public DbSet<sys_codeType> sys_codeType { get; set; }
        public DbSet<sys_log> sys_log { get; set; }
        public DbSet<sys_loginHistory> sys_loginHistory { get; set; }
        public DbSet<sys_menu> sys_menu { get; set; }
        public DbSet<sys_menuButtonMap> sys_menuButtonMap { get; set; }
        public DbSet<sys_organize> sys_organize { get; set; }
        public DbSet<sys_organizeRoleMap> sys_organizeRoleMap { get; set; }
        public DbSet<sys_parameter> sys_parameter { get; set; }
        public DbSet<sys_permission> sys_permission { get; set; }
        public DbSet<sys_role> sys_role { get; set; }
        public DbSet<sys_roleMenuButtonMap> sys_roleMenuButtonMap { get; set; }
        public DbSet<sys_roleMenuColumnMap> sys_roleMenuColumnMap { get; set; }
        public DbSet<sys_roleMenuMap> sys_roleMenuMap { get; set; }
        public DbSet<sys_rolePermissionMap> sys_rolePermissionMap { get; set; }
        public DbSet<sys_user> sys_user { get; set; }
        public DbSet<sys_userOrganizeMap> sys_userOrganizeMap { get; set; }
        public DbSet<sys_userRoleMap> sys_userRoleMap { get; set; }
        public DbSet<sys_userSetting> sys_userSetting { get; set; }
        public DbSet<sys_serial> sys_serial { get; set; }
        public DbSet<sys_session> sys_session { get; set; }
        public DbSet<sys_file> sys_file { get; set; }
        public DbSet<sys_searchScheme> sys_searchScheme { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new sys_buttonMap());
            modelBuilder.Configurations.Add(new sys_codeMap());
            modelBuilder.Configurations.Add(new sys_codeTypeMap());
            modelBuilder.Configurations.Add(new sys_logMap());
            modelBuilder.Configurations.Add(new sys_loginHistoryMap());
            modelBuilder.Configurations.Add(new sys_menuMap());
            modelBuilder.Configurations.Add(new sys_menuButtonMapMap());
            modelBuilder.Configurations.Add(new sys_organizeMap());
            modelBuilder.Configurations.Add(new sys_organizeRoleMapMap());
            modelBuilder.Configurations.Add(new sys_parameterMap());
            modelBuilder.Configurations.Add(new sys_permissionMap());
            modelBuilder.Configurations.Add(new sys_roleMap());
            modelBuilder.Configurations.Add(new sys_roleMenuButtonMapMap());
            modelBuilder.Configurations.Add(new sys_roleMenuColumnMapMap());
            modelBuilder.Configurations.Add(new sys_roleMenuMapMap());
            modelBuilder.Configurations.Add(new sys_rolePermissionMapMap());
            modelBuilder.Configurations.Add(new sys_userMap());
            modelBuilder.Configurations.Add(new sys_userOrganizeMapMap());
            modelBuilder.Configurations.Add(new sys_userRoleMapMap());
            modelBuilder.Configurations.Add(new sys_userSettingMap());
        }
    }
}
