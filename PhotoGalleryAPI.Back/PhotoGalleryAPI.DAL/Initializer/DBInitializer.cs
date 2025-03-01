using Microsoft.EntityFrameworkCore;
using PhotoGalleryAPI.DAL.DBContext;
using PhotoGalleryAPI.DAL.Entities;
using PhotoGalleryAPI.Shared.Password;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGalleryAPI.DAL.Initializer
{
    public static class DBInitializer
    {
        public static async Task Initialize(AppDbContext context)
        {
            // Seed default roles
            await SeedRolesAsync(context);

            // Seed default admin user
            await SeedAdminUserAsync(context);
        }

        private static async Task SeedRolesAsync(AppDbContext context)
        {
            if (!await context.Roles.AnyAsync())
            {
                await context.Roles.AddAsync(new Role { Id = Guid.NewGuid(), RoleName = "Admin" });
                await context.Roles.AddAsync(new Role { Id = Guid.NewGuid(), RoleName = "User" });
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedAdminUserAsync(AppDbContext context)
        {
            if ((await context.Persons.SingleOrDefaultAsync(p => p.Email == "admin@example.com")) == null)
            {
                var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Admin");
                if (adminRole == null)
                {
                    throw new Exception("Admin role not found.");
                }

                var adminUser = new Person
                {
                    Id = Guid.NewGuid(),
                    Username = "Admin",
                    Email = "admin@example.com",
                    PasswordHash = PassHandler.CreatePasswordHash("Admin@123"), // Hash the password
                    RoleId = adminRole.Id
                };

                await context.Persons.AddAsync(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
}
