﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniBlogWeb.Models.ViewModels;

namespace MiniBlogWeb.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        IdentityUser identityUser = new()
        {
            UserName = registerViewModel.Username,
            Email = registerViewModel.Email
        };

        var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

        if (identityResult.Succeeded)
        {
            // assign this user the "User" role 
            var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");

            if (roleIdentityResult.Succeeded)
            {
                // Show success notification
                return RedirectToAction("Register");
            }
        }
        //Show error message
        return View();
    }

    public IActionResult Login(string ReturnUrl)
    {
        LoginViewModel model = new()
        {
            ReturnUrl = ReturnUrl
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

        if (signInResult != null && signInResult.Succeeded)
        {
            if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
            {
                return RedirectToPage(loginViewModel.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
