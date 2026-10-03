namespace AutoFocusASP.ViewModels;

/// <summary>Backs the admin spreadsheets.</summary>
public class AdminDashboardViewModel
{
    public IReadOnlyList<AdminUserRowViewModel> Users { get; init; } = Array.Empty<AdminUserRowViewModel>();
    public IReadOnlyList<AdminCommentRowViewModel> Comments { get; init; } = Array.Empty<AdminCommentRowViewModel>();
    public IReadOnlyList<AdminCarLikesRowViewModel> CarLikes { get; init; } = Array.Empty<AdminCarLikesRowViewModel>();
}

/// <summary>One row of the user-accounts spreadsheet.</summary>
public class AdminUserRowViewModel
{
    public string UserName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public int ImagesLiked { get; init; }
    public bool IsAdmin { get; init; }
}

/// <summary>One row of the comments spreadsheet.</summary>
public class AdminCommentRowViewModel
{
    public int Id { get; init; }
    public string CarSlug { get; init; } = string.Empty;
    public string CarName { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

/// <summary>One row of the car-likes spreadsheet.</summary>
public class AdminCarLikesRowViewModel
{
    public string CarName { get; init; } = string.Empty;
    public int Likes { get; init; }
}
