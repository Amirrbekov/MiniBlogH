using Microsoft.EntityFrameworkCore;
using MiniBlogWeb.Data;
using MiniBlogWeb.Models.Domain;

namespace MiniBlogWeb.Repositories;

public class BlogPostCommentRepository : IBlogPostCommentRepository
{
    private readonly BlogDbContext _dbContext;
    public BlogPostCommentRepository(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
    {
        await _dbContext.BlogPostComments.AddAsync(blogPostComment);
        _dbContext.SaveChanges();
        return blogPostComment;
    }

    public async Task<IEnumerable<BlogPostComment>> GetCommentByBlogIdAsync(Guid blogPostId) => await _dbContext.BlogPostComments.Where(x => x.BlogPostId == blogPostId).ToListAsync();
}
