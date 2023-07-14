using Microsoft.EntityFrameworkCore;

namespace Blog.Models;

/* add initial data*/

public class BlogPostContext : DbContext
{
    public BlogPostContext(DbContextOptions<BlogPostContext> options)
        : base(options)
    {
    }

    public DbSet<BlogPostItem> TodoItems { get; set; } = null!;
}