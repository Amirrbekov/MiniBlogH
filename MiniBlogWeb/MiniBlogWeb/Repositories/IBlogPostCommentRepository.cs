using MiniBlogWeb.Models.Domain;

namespace MiniBlogWeb.Repositories;

public interface IBlogPostCommentRepository
{
    Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);

    Task<IEnumerable<BlogPostComment>> GetCommentByBlogIdAsync(Guid blogPostId);
}
