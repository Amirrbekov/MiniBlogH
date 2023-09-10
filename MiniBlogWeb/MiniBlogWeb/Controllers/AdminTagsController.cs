using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBlogWeb.Data;
using MiniBlogWeb.Models.Domain;
using MiniBlogWeb.Models.ViewModels;
using MiniBlogWeb.Repositories;

namespace MiniBlogWeb.Controllers;

[Authorize(Roles = "Admin")]
public class AdminTagsController : Controller
{
    private readonly ITagRepository tagRepository;
    public AdminTagsController(ITagRepository tagRepository)
    {
        this.tagRepository = tagRepository;
    }

    public IActionResult Add() => View();

    [HttpPost]
    public async Task<IActionResult> Add(AddTagRequest addTagRequest)
    {
        Tag tag = new Tag
        {
            Name = addTagRequest.Name,
            DisplayName = addTagRequest.DisplayName
        };

        await tagRepository.AddAsync(tag);

        return RedirectToAction("List");
    }

    public async Task<IActionResult> List() => View(await tagRepository.GetAllAsync());

    public async Task<IActionResult> Edit(Guid id)
    {
        Tag tag = await tagRepository.GetAsync(id);
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
    public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
    {
        Tag tag = new()
        {
            Id = editTagRequest.Id,
            Name = editTagRequest.Name,
            DisplayName = editTagRequest.DislayName
        };

        var updatedTag = await tagRepository.UpdateAsync(tag);

        if (updatedTag != null)
        {
            //Show a success notification
        }
        else
        {
            //Show a error notification
        }

        //Show a error notification
        return RedirectToAction("Edit", new { id = editTagRequest.Id });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
    {
        var deletedTag = await tagRepository.DeleteAsync(editTagRequest.Id);

        if (deletedTag != null)
        {
            return RedirectToAction("List");
        }

        //Show a error notofication
        return RedirectToAction("Edit", new { id = editTagRequest.Id });
    }
}
