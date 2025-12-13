using Domain.Entities;
using Domain.Entities.Enum;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class SystemAdminSeedData
{
    public static async Task InitializeAsync(MedShareDbContext context)
    {
        if (await context.Users.AnyAsync(u => u.Role == Role.Admin))
            return;

        var passwordHasher = new PasswordHasher<User>();

        var admins = new List<User>
        {
            new User
            {
                FullName = "System Admin 1",
                Email = "admin1@medshare.jo",
                Role = Role.Admin
            },
            new User
            {
                FullName = "System Admin 2",
                Email = "admin2@medshare.jo",
                Role = Role.Admin
            },
            new User
            {
                FullName = "System Admin 3",
                Email = "admin3@medshare.jo",
                Role = Role.Admin
            },
            new User
            {
                FullName = "System Admin 4",
                Email = "admin4@medshare.jo",
                Role = Role.Admin
            }
        };

        foreach (var admin in admins)
        {
            admin.Password = passwordHasher.HashPassword(admin, "Admin@123");
        }

        await context.Users.AddRangeAsync(admins);
        await context.SaveChangesAsync();
    }
}
