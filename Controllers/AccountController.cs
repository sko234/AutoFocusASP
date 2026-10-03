using AutoFocusASP.Data;
using AutoFocusASP.Models;
using AutoFocusASP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoFocusASP.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly AppDbContext _db;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        AppDbContext db)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _db = db;
    }

    [HttpGet("/register")]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost("/register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        // The administrator account is seeded, never signed up.
        if (string.Equals(model.Email.Trim(), AdminSeed.AdminEmail, StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        if (string.Equals(model.DisplayName.Trim(), "admin", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError(nameof(model.DisplayName), "That display name is not available.");
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            DisplayName = model.DisplayName.Trim()
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        // Matches the original flow: sign up and land straight on the dashboard.
        await _signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToAction(nameof(Dashboard));
    }

    [HttpGet("/login")]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost("/login")]
    [ActionName("Login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginPost(LoginViewModel model, string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);

        // The administrator signs in only from /admin. Here an admin account is
        // treated exactly like an unknown one, so this form never reveals it.
        if (user is not null && !await _userManager.IsInRoleAsync(user, AdminSeed.AdminRole))
        {
            var result = await _signInManager.PasswordSignInAsync(
                user.UserName ?? model.Email,
                model.Password,
                isPersistent: model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return LocalRedirect(SafeReturnUrl(returnUrl));
            }
        }

        // One message for both "no such account" and "wrong password" so the form
        // never reveals which addresses are registered.
        ModelState.AddModelError(string.Empty, "Invalid email or password.");
        return View(model);
    }

    [HttpPost("/logout")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("/dashboard")]
    [Authorize]
    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return Challenge();

        var mine = await _db.Comments
            .AsNoTracking()
            .Where(c => c.AuthorId == user.Id)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        var bySlug = CarData.All.ToDictionary(c => c.Slug, c => c.Name);

        return View(new DashboardViewModel
        {
            DisplayName = user.AuthorName,
            Email = user.Email ?? string.Empty,
            Comments = mine
                .Select(c => new DashboardCommentViewModel
                {
                    Id = c.Id,
                    CarSlug = c.CarSlug,
                    CarName = bySlug.TryGetValue(c.CarSlug, out var n) ? n : c.CarSlug,
                    Text = c.Text,
                    CreatedAt = c.CreatedAt
                })
                .ToList()
        });
    }

    /// <summary>
    /// Only ever redirect back inside this site, so a crafted ?returnUrl= cannot
    /// bounce a freshly signed-in account to another origin.
    /// </summary>
    private string SafeReturnUrl(string? returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return returnUrl;
        }
        return Url.Action(nameof(Dashboard)) ?? "/dashboard";
    }
}
