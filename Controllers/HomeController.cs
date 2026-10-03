using AutoFocusASP.Data;
using AutoFocusASP.Models;
using AutoFocusASP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoFocusASP.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public HomeController(
        AppDbContext db,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _db = db;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    private async Task<HashSet<string>> GetAdminAuthorIds()
    {
        var admins = await _userManager.GetUsersInRoleAsync(AdminSeed.AdminRole);
        return admins.Select(user => user.Id).ToHashSet();
    }

    /// <summary>Home page: the hero image grid above the three brand tiles.</summary>
    [HttpGet("/")]
    public IActionResult Index()
    {
        // The preloader is a home-page flourish; every other route hides it at once.
        ViewBag.IsHome = true;
        ViewBag.HideFooter = false;
        return View(new HomeViewModel { Cars = CarData.All });
    }

    [HttpGet("/ferrari")]
    public IActionResult Ferrari() => BrandIndex("Ferrari");

    [HttpGet("/lamborghini")]
    public IActionResult Lamborghini() => BrandIndex("Lamborghini");

    [HttpGet("/porsche")]
    public IActionResult Porsche() => BrandIndex("Porsche");

    [HttpGet("/mclaren")]
    public IActionResult McLaren() => BrandIndex("McLaren");

    [HttpGet("/bugatti")]
    public IActionResult Bugatti() => BrandIndex("Bugatti");

    private IActionResult BrandIndex(string brand)
    {
        ViewBag.IsHome = false;
        ViewBag.HideFooter = true;
        return View("Brand", new BrandViewModel
        {
            Brand = brand,
            Cars = CarData.ByBrand(brand)
        });
    }

    /// <summary>A single car: slideshow, listings panel, specifications and comments.</summary>
    [HttpGet("/car/{slug}")]
    public async Task<IActionResult> CarDetails(string slug)
    {
        var car = CarData.Find(slug);
        if (car is null) return NotFound();

        ViewBag.IsHome = false;
        ViewBag.HideFooter = true;

        var comments = _db.Comments
            .Where(c => c.CarSlug == slug)
            .OrderByDescending(c => c.CreatedAt)
            .AsNoTracking()
            .ToList();

        if (User.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is not null)
            {
                ViewBag.IsCarouselLiked = await _db.ImageLikes
                    .AnyAsync(l => l.UserId == user.Id && l.CarSlug == car.Slug);
            }
        }

        return View(new CarDetailsViewModel
        {
            Car = car,
            Comments = comments,
            AdminAuthorIds = await GetAdminAuthorIds()
        });
    }

    /// <summary>Adds a comment to a car's page. Signed-in accounts only.</summary>
    [HttpPost("/car/{slug}/comment")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PostComment(string slug, CommentViewModel model)
    {
        var car = CarData.Find(slug);
        if (car is null) return NotFound();

        // The car comes from the route; never trust the form field.
        model.CarSlug = slug;

        if (model.ParentCommentId.HasValue)
        {
            var parent = await _db.Comments.AsNoTracking().FirstOrDefaultAsync(c =>
                c.Id == model.ParentCommentId.Value &&
                c.CarSlug == slug &&
                c.ParentCommentId == null);

            if (parent is null)
            {
                ModelState.AddModelError(nameof(model.ParentCommentId), "That comment cannot be replied to.");
            }
        }

        if (!ModelState.IsValid)
        {
            var comments = _db.Comments
                .Where(c => c.CarSlug == slug)
                .OrderByDescending(c => c.CreatedAt)
                .AsNoTracking()
                .ToList();

            return View("CarDetails", new CarDetailsViewModel
            {
                Car = car,
                Comments = comments,
                AdminAuthorIds = await GetAdminAuthorIds()
            });
        }

        var user = await _userManager.GetUserAsync(User);
        if (user is null) return Challenge();

        var trimmed = model.Text.Trim();
        if (trimmed.Length == 0)
        {
            ModelState.AddModelError(nameof(model.Text), "A comment cannot be empty.");
            return RedirectToAction(nameof(CarDetails), new { slug });
        }

        _db.Comments.Add(new Comment
        {
            CarSlug = slug,
            AuthorId = user.Id,
            AuthorName = user.AuthorName,
            Text = trimmed,
            CreatedAt = DateTime.UtcNow,
            ParentCommentId = model.ParentCommentId
        });

        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(CarDetails), new { slug });
    }

    /// <summary>Toggles one authenticated user's like for the entire car carousel.</summary>
    [HttpPost("/car/{slug}/like")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LikeImage(string slug, int imageIndex)
    {
        var car = CarData.Find(slug);
        if (car is null || imageIndex < 0 || imageIndex >= car.Images.Count)
            return BadRequest();

        var user = await _userManager.GetUserAsync(User);
        if (user is null) return Challenge();

        var alreadyLiked = await _db.ImageLikes.AnyAsync(l =>
            l.UserId == user.Id && l.CarSlug == car.Slug);

        if (alreadyLiked)
        {
            var likes = await _db.ImageLikes
                .Where(l => l.UserId == user.Id && l.CarSlug == car.Slug)
                .ToListAsync();
            _db.ImageLikes.RemoveRange(likes);
            await _db.SaveChangesAsync();
            return Json(new { liked = false });
        }

        _db.ImageLikes.Add(new ImageLike
        {
            UserId = user.Id,
            CarSlug = car.Slug,
                ImageIndex = 0,
            CreatedAt = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();

        return Json(new { liked = true });
    }

    /// <summary>Deletes one of the signed-in account's own comments.</summary>
    [HttpPost("/car/{slug}/comment/{id}/delete")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteComment(string slug, int id, string? returnUrl = null)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return Challenge();

        var comment = await _db.Comments.FindAsync(id);

        // Silently ignore anything that is not the caller's own comment, so the
        // endpoint cannot be used to probe for other people's comment ids.
        if (comment is null || (comment.AuthorId != user.Id && !User.IsInRole(AdminSeed.AdminRole)))
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);
            return RedirectToAction(nameof(CarDetails), new { slug });
        }

        var replies = await _db.Comments
            .Where(c => c.ParentCommentId == comment.Id)
            .ToListAsync();
        _db.Comments.RemoveRange(replies);
        _db.Comments.Remove(comment);
        await _db.SaveChangesAsync();

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return LocalRedirect(returnUrl);
        return RedirectToAction(nameof(CarDetails), new { slug });
    }

    [HttpGet("/Home/Error")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public IActionResult Error(int? code = null)
    {
        ViewBag.IsHome = false;
        ViewBag.HideFooter = false;
        ViewBag.StatusCode = code;
        return View("~/Views/Shared/Error.cshtml");
    }
}
