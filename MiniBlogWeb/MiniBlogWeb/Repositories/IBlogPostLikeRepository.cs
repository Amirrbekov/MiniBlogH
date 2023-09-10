using MiniBlogWeb.Models.Domain;

namespace MiniBlogWeb.Repositories;

public interface IBlogPostLikeRepository
{
    Task<int> GetTotalLikes(Guid blogPostId);

    Task<IEnumerable<BlogPostLike>> GetLikesForBlogForUsers(Guid blogPostId);

    Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);
}
