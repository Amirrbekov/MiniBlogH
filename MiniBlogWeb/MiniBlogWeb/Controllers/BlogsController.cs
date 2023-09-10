using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniBlogWeb.Models.Domain;
using MiniBlogWeb.Models.ViewModels;
using MiniBlogWeb.Repositories;

namespace MiniBlogWeb.Controllers;

public class BlogsController : Controller
{
    private readonly IBlogPostRepository blogPostRepository;
    private readonly IBlogPostLikeRepository blogLikeRepository;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly UserManager<IdentityUser> userManager;
    private readonly IBlogPostCommentRepository blogPostCommentRepository;

    public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogLikeRepository, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IBlogPostCommentRepository blogPostCommentRepository)
    {
        this.blogPostRepository = blogPostRepository;
        this.blogLikeRepository = blogLikeRepository;
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.blogPostCommentRepository = blogPostCommentRepository;

    }

    public async Task<IActionResult> Index(string UrlHandle)
    {
        var liked = false;
        var blogPost = await blogPostRepository.GetByUrlHandleAsync(UrlHandle);
        BlogDetailViewModel blogPostLikesViewModel = new();

        if (blogPost != null)
        {
            var totalLikes = await blogLikeRepository.GetTotalLikes(blogPost.Id);

            if (signInManager.IsSignedIn(User))
            {
                // Get like for this blog for this user

                var likesForBlog = await blogLikeRepository.GetLikesForBlogForUsers(blogPost.Id);

                var userId = userManager.GetUserId(User);

                if (userId != null)
                {
                    var likesFromUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                    liked = likesFromUser != null;
                }
            }

            // Get comments for blog post
            var blogCommentsModel = await blogPostCommentRepository.GetCommentByBlogIdAsync(blogPost.Id);

            List<BlogComment> blogCommentsForView = new();

            foreach (var comment in blogCommentsModel)
            {
                blogCommentsForView.Add(new BlogComment
                {
                    Description = comment.Description,
                    DateAdded = comment.DateAdded,
                    Username = (await userManager.FindByIdAsync(comment.UserId.ToString())).UserName
                });
            }

            blogPostLikesViewModel = new()
            {
                Id = blogPost.Id,
                Content = blogPost.Content,
                PageTitle = blogPost.PageTitle,
                Author = blogPost.Author,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Heading = blogPost.Heading,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                Visible = blogPost.Visible,
                Tags = blogPost.Tags,
                TotalLikes = totalLikes,
                Liked = liked,
                Comments = blogCommentsForView
            };
        }

        return View(blogPostLikesViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Index(BlogDetailViewModel blogDetailViewModel)
    {
        if (signInManager.IsSignedIn(User))
        {
            BlogPostComment domainModel = new()
            {
                BlogPostId = blogDetailViewModel.Id,
                Description = blogDetailViewModel.CommentDescription,
                UserId = Guid.Parse(userManager.GetUserId(User))
            };

            await blogPostCommentRepository.AddAsync(domainModel);

            return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailViewModel.UrlHandle });
        }

        return View();
    }
}
