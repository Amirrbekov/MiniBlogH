using Microsoft.AspNetCore.Mvc;
using MiniBlogWeb.Data;
using MiniBlogWeb.Models.Domain;
using MiniBlogWeb.Models.ViewModels;

namespace MiniBlogWeb.Controllers;

public class AdminTagsController : Controller
{
    private readonly BlogDbContext _blogDbContext;
    public AdminTagsController(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }

    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(AddTagRequest addTagRequest)
    {
        Tag tag = new Tag
        {
            Name = addTagRequest.Name,
            DisplayName = addTagRequest.DisplayName
        };

        _blogDbContext.Tags.Add(tag);
        _blogDbContext.SaveChanges();

        return RedirectToAction("List");
    }

    public IActionResult List() => View(_blogDbContext.Tags.ToList());

    public IActionResult Edit(Guid id)
    {
        Tag tag = _blogDbContext.Tags.FirstOrDefault(t => t.Id == id);
        if (tag != null)
        {
            EditTagRequest editTagRequest = new()
            {
                Id = tag.Id,
                Name = tag.Name,
                DislayName = tag.DisplayName
            };

            return View(editTagRequest);
        }

        return View(null);
    }

    [HttpPost]
    public IActionResult Edit(EditTagRequest editTagRequest)
    {
        Tag tag = new()
        {
            Id = editTagRequest.Id,
            Name = editTagRequest.Name,
            DisplayName = editTagRequest.DislayName
        };

        Tag existingTag = _blogDbContext.Tags.Find(tag.Id);

        if (existingTag != null)
        {
            existingTag.Name = tag.Name;
            existingTag.DisplayName = tag.DisplayName;
            _blogDbContext.SaveChanges();

            //Show a success notification
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
        //Show a error notification
        return RedirectToAction("Edit", new { id = editTagRequest.Id });
    }

    [HttpPost]
    public IActionResult Delete(EditTagRequest editTagRequest)
    {
        Tag tag = _blogDbContext.Tags.Find(editTagRequest.Id);
        if (tag != null)
        {
            _blogDbContext.Tags.Remove(tag);
            _blogDbContext.SaveChanges();

            //Show a success notofication
            return RedirectToAction("List");
        }
        //Show a error notofication
        return RedirectToAction("Edit", new { id = editTagRequest.Id });
    }
}
