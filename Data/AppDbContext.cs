using AutoFocusASP.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutoFocusASP.Data;

/// <summary>
/// EF Core context backing the SQLite file <c>app.db</c>. It derives from
/// <see cref="IdentityDbContext{TUser}"/> so ASP.NET Core Identity stores its
/// accounts, roles, claims and logins in the same database as the comments.
/// </summary>
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Comment> Comments => Set<Comment>();

    public DbSet<ImageLike> ImageLikes => Set<ImageLike>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Comment>(comment =>
        {
            comment.HasKey(c => c.Id);
            comment.Property(c => c.CarSlug).IsRequired().HasMaxLength(64);
            comment.Property(c => c.AuthorName).IsRequired().HasMaxLength(60);
            comment.Property(c => c.Text).IsRequired().HasMaxLength(1000);

            // A car's page is read newest-first, so index the slug and the ordering.
            comment.HasIndex(c => new { c.CarSlug, c.CreatedAt });

            comment.HasOne(c => c.Author)
                  .WithMany(u => u.Comments)
                  .HasForeignKey(c => c.AuthorId)
                  .OnDelete(DeleteBehavior.Cascade);

            comment.HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ImageLike>(like =>
        {
            like.HasKey(l => l.Id);
            like.Property(l => l.CarSlug).IsRequired().HasMaxLength(64);
            like.HasIndex(l => new { l.UserId, l.CarSlug, l.ImageIndex }).IsUnique();

            like.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
