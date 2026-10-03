using AutoFocusASP.Data;
using AutoFocusASP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Data ------------------------------------------------------------------
// app.db sits next to the project file, matching the checked-in database.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Data Source=app.db"));

// --- Identity --------------------------------------------------------------
// Password rules mirror the strength meter the sign-up form has always shown:
// at least six characters with an uppercase, a lowercase, a digit and a symbol.
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Where the Identity cookie sends people who are not signed in (or not allowed in).
// Anything under /admin goes to the separate admin login; everything else uses the
// public /login page (the default /Account/Login path has no route in this app).
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/login";

    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.Redirect(
            context.Request.Path.StartsWithSegments("/admin") ? "/admin/login" : context.RedirectUri);
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.Redirect(
            context.Request.Path.StartsWithSegments("/admin") ? "/admin/login" : context.RedirectUri);
        return Task.CompletedTask;
    };
});

builder.Services.AddControllersWithViews();

// One role guards the admin area; every other account is a plain user.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AdminSeed.AdminOnlyPolicy, policy =>
        policy.RequireAuthenticatedUser().RequireRole(AdminSeed.AdminRole));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/Error", "?code={0}");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Every public page is attribute-routed to a clean URL; this convention route
// is the fallback and also keeps tag helpers such as asp-area happy.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Create app.db on first run, then make sure the admin account exists.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    // EnsureCreated never adds tables to a database that already exists, so an
    // app.db created before ImageLikes was introduced would not have it.
    db.Database.ExecuteSqlRaw("""
        CREATE TABLE IF NOT EXISTS "ImageLikes" (
            "Id" INTEGER NOT NULL CONSTRAINT "PK_ImageLikes" PRIMARY KEY AUTOINCREMENT,
            "UserId" TEXT NOT NULL,
            "CarSlug" TEXT NOT NULL,
            "ImageIndex" INTEGER NOT NULL,
            "CreatedAt" TEXT NOT NULL,
            CONSTRAINT "FK_ImageLikes_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
        )
        """);

    db.Database.ExecuteSqlRaw("""
        CREATE UNIQUE INDEX IF NOT EXISTS "IX_ImageLikes_UserId_CarSlug_ImageIndex"
        ON "ImageLikes" ("UserId", "CarSlug", "ImageIndex")
        """);

    using var connection = db.Database.GetDbConnection();
    connection.Open();
    using var columnCheck = connection.CreateCommand();
    columnCheck.CommandText = "SELECT COUNT(*) FROM pragma_table_info('Comments') WHERE name = 'ParentCommentId'";
    var hasParentCommentId = Convert.ToInt32(columnCheck.ExecuteScalar()) > 0;
    if (!hasParentCommentId)
    {
        db.Database.ExecuteSqlRaw("ALTER TABLE \"Comments\" ADD COLUMN \"ParentCommentId\" INTEGER NULL");
    }
}

await AdminSeed.SeedAsync(app.Services);

app.Run();