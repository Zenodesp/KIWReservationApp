using KIWReservationApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KIWReservationApp.Data;

public class IdentityContext : IdentityDbContext<AppUser>
{
    public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
    {
    }

    public static async Task SeedRolesAndUsers(IdentityContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        if(!context.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole { Name = "Admin", Id = "Admin", NormalizedName = "Admin"});
            context.SaveChanges();
        }

        if (!context.Users.Any())
        {
            AppUser dummyUser = new AppUser
            {
                Id = "Dummy",
                Email = "Dummy@dumdum.com",
                UserName = "Dummy",
                FirstName = "Dummy",
                LastName = "Dummy",
                
                PasswordHash = "Dummy",
                LockoutEnabled = true,
                LockoutEnd = DateTime.MaxValue
            };
            AppUser AdminUser = new AppUser
            {
                Id = "Admin",
                Email = "Admin@Admin.com",
                UserName = "Admin",
                FirstName = "Admin",
                LastName = "Admin",
                
                PasswordHash = "Admin",
                LockoutEnabled = true,
                LockoutEnd = DateTime.MaxValue
            };

            var result = await userManager.CreateAsync(AdminUser, "Abc!12345");

            context.Users.Add(dummyUser);
            context.SaveChanges();


        }

        AppUser adminuser = await userManager.FindByNameAsync("Admin");
        if (!await userManager.IsInRoleAsync(adminuser, "Admin"))
        {
            context.UserRoles.Add(new IdentityUserRole<string> { UserId = adminuser.Id, RoleId = "Admin" });
            context.SaveChanges();
        }





    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        //remember to change the connection string to the correct one once the app is deployed!
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=KIWReservationAppContext-8b685063-635d-422f-be35-5f54e7f984a6;Trusted_Connection=True;MultipleActiveResultSets=true");
    }

    internal static object DataInitialiser(IdentityContext identityContext)
    {
        throw new NotImplementedException();
    }
}
