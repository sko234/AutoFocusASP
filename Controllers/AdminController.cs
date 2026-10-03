using AutoFocusASP.Data;
using AutoFocusASP.Models;
using AutoFocusASP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoFocusASP.Controllers;

public class AdminController : Controller
{
    private readonly AppDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AdminController(AppDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _db = db;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    private async Task<List<string>> GetAdminIdsAsync() =>
        (await _userManager.GetUsersInRoleAsync(AdminSeed.AdminRole)).Select(u => u.Id).ToList();

    [HttpGet("/admin")]
    [Authorize(Policy = AdminSeed.AdminOnlyPolicy)]
    public async Task<IActionResult> Index()
    {
        var adminIds = await GetAdminIdsAsync();
        var users = await _db.Users.AsNoTracking()
            .Where(u => !adminIds.Contains(u.Id))
            .OrderBy(u => u.DisplayName)
            .Select(u => new { u.Id, u.DisplayName, u.Email })
            .ToListAsync();

        var userIds = users.Select(u => u.Id).ToList();
        var userNames = users.ToDictionary(
            u => u.Id,
            u => string.IsNullOrWhiteSpace(u.DisplayName) ? (u.Email ?? string.Empty) : u.DisplayName);
        foreach (var adminId in adminIds)
        {
            userNames[adminId] = "admin";
        }

        var comments = await _db.Comments.AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        var commentRows = comments.Select(c => new AdminCommentRowViewModel
        {
            Id = c.Id,
            CarSlug = c.CarSlug,
            CarName = CarData.Find(c.CarSlug)?.Name ?? c.CarSlug,
            Text = c.Text,
            UserName = userNames.GetValueOrDefault(c.AuthorId, string.Empty),
            CreatedAt = c.CreatedAt
        }).ToList();

        var likeCounts = await _db.ImageLikes.AsNoTracking()
            .Where(l => userIds.Contains(l.UserId))
            .Select(l => new { l.UserId, l.CarSlug })
            .Distinct()
            .GroupBy(l => l.UserId)
            .Select(g => new { UserId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.UserId, x => x.Count);

        var userRows = users.Select(user => new AdminUserRowViewModel
        {
            UserName = string.IsNullOrWhiteSpace(user.DisplayName) ? (user.Email ?? string.Empty) : user.DisplayName,
            Email = user.Email ?? string.Empty,
            ImagesLiked = likeCounts.GetValueOrDefault(user.Id)
        }).Prepend(new AdminUserRowViewModel
        {
            UserName = "admin",
            IsAdmin = true
        }).ToList();

        var carLikeCounts = await _db.ImageLikes.AsNoTracking()
            .Select(l => new { l.UserId, l.CarSlug })
            .Distinct()
            .GroupBy(l => l.CarSlug)
            .ToDictionaryAsync(g => g.Key, g => g.Count());

        var carLikeRows = CarData.All.Select(car => new AdminCarLikesRowViewModel
        {
            CarName = car.Name,
            Likes = carLikeCounts.GetValueOrDefault(car.Slug)
        }).ToList();

        return View("Dashboard", new AdminDashboardViewModel
        {
            Users = userRows,
            Comments = commentRows,
            CarLikes = carLikeRows
        });
    }

    [HttpGet("/admin/login")]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true && User.IsInRole(AdminSeed.AdminRole))
            return RedirectToAction(nameof(Index));
        return View(new LoginViewModel());
    }

    [HttpPost("/admin/login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is not null && await _userManager.IsInRoleAsync(user, AdminSeed.AdminRole))
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(nameof(Index));
            }
        }
        ModelState.AddModelError(string.Empty, "Invalid email or password.");
        return View(model);
    }
}
