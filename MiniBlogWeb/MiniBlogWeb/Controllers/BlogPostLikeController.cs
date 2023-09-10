using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniBlogWeb.Models.Domain;
using MiniBlogWeb.Models.ViewModels;
using MiniBlogWeb.Repositories;

namespace MiniBlogWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogPostLikeController : ControllerBase
{
    private readonly IBlogPostLikeRepository blogPostLikeRepository;
    public BlogPostLikeController(IBlogPostLikeRepository blogPostLikeRepository)
    {
        this.blogPostLikeRepository = blogPostLikeRepository;
    }

    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
    {
        BlogPostLike model = new()
        {
            BlogPostId = addLikeRequest.BlogPostId,
            UserId = addLikeRequest.UserId
        };

        await blogPostLikeRepository.AddLikeForBlog(model);

        return Ok();
    }

    [Route("{blogPostId.Guid}/totalLike")]
    public async Task<IActionResult> GetTotalLikeForBlog([FromRoute] Guid blogPostId)
    {
        var totalLikes = await blogPostLikeRepository.GetTotalLikes(blogPostId);

        return Ok(totalLikes);
    }
}
