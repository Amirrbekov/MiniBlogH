using Microsoft.EntityFrameworkCore;
using MiniBlogWeb.Models.Domain;

namespace MiniBlogWeb.Data;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {
    }

    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<BlogPostLike> BlogPostLikes { get; set; }
    public DbSet<BlogPostComment> BlogPostComments { get; set; }
}
