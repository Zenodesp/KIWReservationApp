using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using KIWReservationApp.Data;
using Microsoft.AspNetCore.Identity;
using KIWReservationApp.Areas.Identity.Data;

namespace KIWReservationApp
{
    public class Program
    {
        public static void Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            builder.Services.AddDbContext<KIWReservationAppContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("KIWReservationAppContext") ?? throw new InvalidOperationException("Connection string 'KIWReservationAppContext' not found.")));

            builder.Services.AddDbContext<IdentityContext>();

            builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>().AddEntityFrameworkStores<IdentityContext>()
                ;

            //add authentication for Microsoft
            //NOTE: in order to get to the secrets of this application, right click on the project, then select 'Manage User Secrets'
            builder.Services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"];
                microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
            });



            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<KIWReservationAppContext>();
                KIWReservationAppContext.DataInitialiser(context).Wait();

                var identityContext = services.GetRequiredService<IdentityContext>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                IdentityContext.SeedRolesAndUsers(identityContext, userManager, roleManager).Wait();

                
                
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

            app.Run();
        }
    }
}