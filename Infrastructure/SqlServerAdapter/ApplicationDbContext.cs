
using ApplicationCore.DomainModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SqlServerAdapter
{
    public class ApplicationDbContext : IdentityDbContext<SysCustomUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<SysApiLog> SysApiLogs { get; set; }
        public DbSet<SysApiUser> SysApiUsers { get; set; }
        public DbSet<SysException> SysExceptions { get; set; }
        public DbSet<SysExpression> SysExpressions { get; set; }
        public DbSet<SysFaq> SysFaqs { get; set; }
        public DbSet<SysHelp> SysHelps { get; set; }
        public DbSet<SysLog> SysLogs { get; set; }
        public DbSet<SysParameter> SysParameters { get; set; }
        public DbSet<SysReleaseNote> SysReleaseNotes { get; set; }
        public DbSet<SysSmtp> SysSmtps { get; set; }
        public DbSet<SysTenant> SysTenants { get; set; }
        public DbSet<SysTenantUser> SysTenantUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
