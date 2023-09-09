using Microsoft.AspNetCore.Mvc;
using MiniBlogWeb.Models.Domain;
using MiniBlogWeb.Repositories;

namespace MiniBlogWeb.Controllers;

public class BlogsController : Controller
{
    private readonly IBlogPostRepository blogPostRepository;

    public BlogsController(IBlogPostRepository blogPostRepository)
    {
        this.blogPostRepository = blogPostRepository;
    }

    public async Task<IActionResult> Index(string UrlHandle) => View(await blogPostRepository.GetByUrlHandleAsync(UrlHandle));
}
