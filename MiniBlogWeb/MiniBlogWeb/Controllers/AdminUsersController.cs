using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniBlogWeb.Models.Domain;
using MiniBlogWeb.Models.ViewModels;
using MiniBlogWeb.Repositories;

namespace MiniBlogWeb.Controllers;
[Authorize(Roles = "Admin")]
public class AdminUsersController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminUsersController(IUserRepository userRepository, UserManager<IdentityUser> userManager)
    {
        _userRepository = userRepository;
    }
    public async Task<IActionResult> List()
    {
        var users = await _userRepository.GetAll();

        var usersViewModel = new UsersViewModel();
        usersViewModel.Users = new List<User>();
        foreach (var user in users)
        {
            usersViewModel.Users.Add(new User
            {
                Id = Guid.Parse(user.Id),
                Username = user.UserName,
                EmailAddress = user.Email,
            });
        }

        return View(usersViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> List(UsersViewModel usersViewModel)
    {
        var identityUser = new IdentityUser
        {
            UserName = usersViewModel.Username,
            Email = usersViewModel.Email,
        };

        var identityResult = await _userManager.CreateAsync(identityUser, usersViewModel.Password);
        if (identityResult is not null)
        {
            if (identityResult.Succeeded)
            {
                // asign roles to this user
                var roles = new List<string> { "User" };

                if (usersViewModel.AdminroleChechbox)
                {
                    roles.Add("Admin");
                }

                identityResult = await _userManager.AddToRolesAsync(identityUser, roles);

                if (identityResult is not null)
                {
                    return RedirectToAction("List", "AdminUsers");
                }
            }
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user is not null)
        {
            var identityResult = await _userManager.DeleteAsync(user);

            if (identityResult is not null && identityResult.Succeeded)
            {
                return RedirectToAction("List", "AdminUsers");
            }
        }

        return View();
    }
}
