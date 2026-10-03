namespace AutoFocusASP.Models;

/// <summary>
/// One image of one car liked by one account. The car is referenced by slug (the
/// catalogue is hardcoded in <see cref="Data.CarData"/>) and the image by its
/// position in that car's image list.
/// </summary>
public class ImageLike
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    /// <summary>Slug of the car, e.g. "porsche-911-turbo-s".</summary>
    public string CarSlug { get; set; } = string.Empty;

    /// <summary>Zero-based position of the image in the car's slideshow.</summary>
    public int ImageIndex { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}