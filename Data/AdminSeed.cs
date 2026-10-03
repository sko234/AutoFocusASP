using AutoFocusASP.Models;
using Microsoft.AspNetCore.Identity;

namespace AutoFocusASP.Data;

/// <summary>
/// Creates the single administrator account on startup. Admin is not registered
/// through the sign-up form: it is seeded here so its role cannot be given away by
/// anyone who can reach <c>/register</c>.
/// </summary>
public static class AdminSeed
{
    public const string AdminRole = "Admin";
    public const string AdminOnlyPolicy = "AdminOnly";

    public const string AdminUserName = "Admin";
    public const string AdminEmail = "atanastanev362@gmail.com";
    public const string AdminPassword = "Zaf49825";

    /// <summary>
    /// Ensures the Admin role exists and the administrator account is in it. Safe to
    /// run on every start: existing rows are left untouched.
    /// </summary>
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var provider = scope.ServiceProvider;

        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();

        if (!await roleManager.RoleExistsAsync(AdminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(AdminRole));
        }

        var admin = await userManager.FindByNameAsync(AdminUserName)
                    ?? await userManager.FindByEmailAsync(AdminEmail);

        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = AdminUserName,
                Email = AdminEmail,
                DisplayName = AdminUserName,
                EmailConfirmed = true
            };

            var created = await userManager.CreateAsync(admin);
            if (!created.Succeeded)
            {
                throw new InvalidOperationException(
                    "Could not seed the admin account: " +
                    string.Join("; ", created.Errors.Select(e => e.Description)));
            }
        }
        if (!await userManager.IsInRoleAsync(admin, AdminRole))
        {
            await userManager.AddToRoleAsync(admin, AdminRole);
        }

        // The password hash is written directly on every start, so an account left
        // with a hash that no longer matches is repaired here. This bypasses the
        // sign-up policy on purpose: this password has no symbol. It can only ever
        // be set from this file, never by a user.
        admin.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(admin, AdminPassword);
        admin.SecurityStamp = Guid.NewGuid().ToString();

        var updated = await userManager.UpdateAsync(admin);
        if (!updated.Succeeded)
        {
            throw new InvalidOperationException(
                "Could not set the admin password: " +
                string.Join("; ", updated.Errors.Select(e => e.Description)));
        }
    }
}
