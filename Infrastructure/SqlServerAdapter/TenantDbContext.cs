using ApplicationCore.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SqlServerAdapter;

public class TenantDbContext : DbContext
{
    private string ConnectionString;
    //private string ConnectionString = "Data Source=.;Initial Catalog=PmaTenant_1;TrustServerCertificate=True;Persist Security Info=True;User ID=pmadbuser;Password=W8Sv!8P9J6i86SyH";
    //public TenantDbContext()
    public TenantDbContext(TenantProvider tenantProvider)
    {
        ConnectionString = tenantProvider.GetConnectionString();
    }

    public DbSet<SysLog> Logs { get; set; }
    public DbSet<SysApiLog> ApiLogs { get; set; }
    public DbSet<SysSmtp> Smtps { get; set; }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<DepartmentCustomer> DepartmentCustomers { get; set; }
    public DbSet<DepartmentMember> DepartmentMembers { get; set; }
    public DbSet<DepartmentRole> DepartmentRoles { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Sprint> Sprints { get; set; }
    public DbSet<Issue> Issues { get; set; }
    public DbSet<SprintTask> SprintTasks { get; set; }
    public DbSet<TaskAssignee> TaskAssignees { get; set; }
    public DbSet<Activity> Activities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!string.IsNullOrEmpty(ConnectionString))
            optionsBuilder.UseSqlServer(ConnectionString);
    }
    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");
        base.ConfigureConventions(builder);
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}