using System.ComponentModel.DataAnnotations;
using AutoFocusASP.Data;
using AutoFocusASP.Models;

namespace AutoFocusASP.ViewModels;

/// <summary>Backs the sign-up form on <c>/register</c>.</summary>
public class RegisterViewModel
{
    [Required]
    [StringLength(60, MinimumLength = 2)]
    [Display(Name = "DISPLAY NAME")]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Display(Name = "EMAIL")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "PASSWORD")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "CONFIRM PASSWORD")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>Backs the sign-in form on <c>/login</c>.</summary>
public class LoginViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "EMAIL")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "PASSWORD")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "REMEMBER ME")]
    public bool RememberMe { get; set; }
}

/// <summary>Backs the comment form posted from a car page.</summary>
public class CommentViewModel
{
    /// <summary>Set from the route, not from the form, so it cannot be spoofed.</summary>
    public string CarSlug { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, MinimumLength = 1)]
    public string Text { get; set; } = string.Empty;

    public int? ParentCommentId { get; set; }
}

/// <summary>Backs the home page: the hero grid plus the three brand tiles.</summary>
public class HomeViewModel
{
    /// <summary>The nine cars shown in the hero image grid, in catalogue order.</summary>
    public IReadOnlyList<CarData.Car> Cars { get; init; } = CarData.All;
}

/// <summary>Backs <c>/ferrari</c>, <c>/lamborghini</c> and <c>/porsche</c>.</summary>
public class BrandViewModel
{
    public string Brand { get; init; } = string.Empty;

    public IReadOnlyList<CarData.Car> Cars { get; init; } = Array.Empty<CarData.Car>();
}

/// <summary>Backs a car page: the slideshow, the specs and the comment thread.</summary>
public class CarDetailsViewModel
{
    public CarData.Car Car { get; init; } = null!;

    /// <summary>Comments on this car, newest first.</summary>
    public IReadOnlyList<Comment> Comments { get; init; } = Array.Empty<Comment>();

    public IReadOnlySet<string> AdminAuthorIds { get; init; } = new HashSet<string>();
}

/// <summary>One row on the dashboard, with the car name already resolved.</summary>
public class DashboardCommentViewModel
{
    public int Id { get; init; }

    public string CarName { get; init; } = string.Empty;

    public string CarSlug { get; init; } = string.Empty;

    public string Text { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }
}

/// <summary>Backs the dashboard: the signed-in account and everything it has posted.</summary>
public class DashboardViewModel
{
    public string DisplayName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public IReadOnlyList<DashboardCommentViewModel> Comments { get; init; } = Array.Empty<DashboardCommentViewModel>();
}
