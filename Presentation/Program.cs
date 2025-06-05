
using ApplicationCore.DomainModel;
using Infrastructure;
using Infrastructure.Captcha;
using Infrastructure.SqlServerAdapter;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("ApplicationConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDbContext<TenantDbContext>();
            builder.Services.AddDefaultIdentity<SysCustomUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager<CustomSignInManager>()
                .AddUserManager<CustomUserManager>();
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            builder.Services.Configure<CaptchaConfig>(builder.Configuration.GetSection("CaptchaConfig"));

            builder.Services.AddTransient<IEmailSender, EmailAdapter>(i =>
                new EmailAdapter(
                    builder.Configuration["Smtp:Host"],
                    builder.Configuration.GetValue<int>("Smtp:Port"),
                    builder.Configuration.GetValue<bool>("Smtp:EnableSSL"),
                    builder.Configuration["Smtp:UserName"],
                    builder.Configuration["Smtp:Password"]
                )
            );
            builder.Services.AddScoped<TenantProvider>();
            builder.Services.AddDependencyInjections();

            // Add services to the container.
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Errors/Index", "?statusCode={0}");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Services.FetchDefaultParameters();
            app.Run();
        }
    }
}