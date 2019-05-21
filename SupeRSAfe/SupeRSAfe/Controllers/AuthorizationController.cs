using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SupeRSAfe.BLL.Interfaces;
using SupeRSAfe.DTO.Manage;

namespace SupeRSAfe.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IUserService _userService;

        public AuthorizationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesEmailExists = await _userService.DoesEmailExists(model.Email);
                if (!doesEmailExists)
                {
                    var result = await _userService.CreateNewUser(model);
                    if (result)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (doesEmailExists)
                {
                    ModelState.AddModelError("", "This email already exists");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            TempData.Clear();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LogIn(model);

                if (result)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong login or(and) password");
                }
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            _userService.LogOut();
            return RedirectToAction("LogIn", "Authorization");
        }
    }
}