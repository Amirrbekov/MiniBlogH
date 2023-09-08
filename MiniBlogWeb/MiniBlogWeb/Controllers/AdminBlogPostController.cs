using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniBlogWeb.Models.Domain;
using MiniBlogWeb.Models.ViewModels;
using MiniBlogWeb.Repositories;

namespace MiniBlogWeb.Controllers;

public class AdminBlogPostController : Controller
{
    private readonly ITagRepository _tagRepository;
    private readonly IBlogPostRepository _blogPostRepository;
    public AdminBlogPostController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
    {
        _tagRepository = tagRepository;
        _blogPostRepository = blogPostRepository;
    }
    public async Task<IActionResult> Add()
    {
        var tags = await _tagRepository.GetAllAsync();

        AddBlogPostRequest model = new()
        {
            Tags = tags.Select(x => new SelectListItem { Text = x.DisplayName, Value = x.Id.ToString() })
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
    {
        BlogPost blogPost = new()
        {
            Heading = addBlogPostRequest.Heading,
            PageTitle = addBlogPostRequest.PageTitle,
            Content = addBlogPostRequest.Content,
            ShortDescription = addBlogPostRequest.ShortDescription,
            FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
            UrlHandle = addBlogPostRequest.UrlHandle,
            PublishedDate = addBlogPostRequest.PublishedDate,
            Author = addBlogPostRequest.Author,
            Visible = addBlogPostRequest.Visible,
        };
        var selectedTags = new List<Tag>();
        foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
        {
            var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
            var existingTag = await _tagRepository.GetAsync(selectedTagIdAsGuid);
            if (existingTag != null)
            {
                selectedTags.Add(existingTag);
            }
        }

        blogPost.Tags = selectedTags;

        await _blogPostRepository.AddAsync(blogPost);

        return RedirectToAction("Add");
    }

    public async Task<IActionResult> List() => View(await _blogPostRepository.GetAllAsync());

    public async Task<IActionResult> Edit(Guid id)
    {
        BlogPost blogPost = await _blogPostRepository.GetAsync(id);
        var tag = await _tagRepository.GetAllAsync();

        if (blogPost != null)
        {
            EditBlogPostRequest editBlogPostRequest = new()
            {
                Id = blogPost.Id,
                Heading = blogPost.Heading,
                PageTitle = blogPost.PageTitle,
                Content = blogPost.Content,
                Author = blogPost.Author,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                ShortDescription = blogPost.ShortDescription,
                PublishedDate = blogPost.PublishedDate,
                Visible = blogPost.Visible,
                Tags = tag.Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() }),
                SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
            };

            return View(editBlogPostRequest);
        }

        return View(null);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
    {
        BlogPost blogPost = new()
        {
            Id = editBlogPostRequest.Id,
            Heading = editBlogPostRequest.Heading,
            PageTitle = editBlogPostRequest.PageTitle,
            Content = editBlogPostRequest.Content,
            Author = editBlogPostRequest.Author,
            FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
            UrlHandle = editBlogPostRequest.UrlHandle,
            ShortDescription = editBlogPostRequest.ShortDescription,
            PublishedDate = editBlogPostRequest.PublishedDate,
            Visible = editBlogPostRequest.Visible,
        };

        var selectedTags = new List<Tag>();

        foreach (var selectedTag in editBlogPostRequest.SelectedTags)
        {
            if (Guid.TryParse(selectedTag, out var tag))
            {
                var foundTag = await _tagRepository.GetAsync(tag);

                if (foundTag != null)
                {
                    selectedTags.Add(foundTag);
                }
            }
        }

        blogPost.Tags = selectedTags;

        var updateBlog = await _blogPostRepository.UpdateAsync(blogPost);

        if (updateBlog != null)
        {
            return RedirectToAction("Edit");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
    {
        var deletedBlogPost = await _blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

        if (deletedBlogPost != null)
        {
            return RedirectToAction("List");
        }

        //Show a error notofication
        return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
    }
}
