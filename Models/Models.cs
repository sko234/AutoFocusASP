using Microsoft.AspNetCore.Identity;

namespace AutoFocusASP.Models;

/// <summary>
/// Registered account. Extends the Identity user with the display name shown
/// next to a comment, and with the comments that account has written.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>Name shown as the comment author. Chosen at registration.</summary>
    public string DisplayName { get; set; } = string.Empty;

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    /// <summary>Name to render on a comment, falling back to the email if unset.</summary>
    public string AuthorName => string.IsNullOrWhiteSpace(DisplayName) ? Email ?? string.Empty : DisplayName;
}

/// <summary>
/// A comment left by a signed-in account on one car's page. The car is referenced
/// by slug rather than by a foreign key because the catalogue in
/// <see cref="Data.CarData"/> is hardcoded, so there is no cars table to join to.
/// </summary>
public class Comment
{
    public int Id { get; set; }

    /// <summary>Slug of the commented car, e.g. "porsche-911-turbo-s".</summary>
    public string CarSlug { get; set; } = string.Empty;

    public string AuthorId { get; set; } = string.Empty;

    /// <summary>Denormalised so a comment still renders if the account is renamed.</summary>
    public string AuthorName { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int? ParentCommentId { get; set; }

    public ApplicationUser? Author { get; set; }

    public Comment? ParentComment { get; set; }

    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
}
