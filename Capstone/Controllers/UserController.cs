using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Capstone.Models;
using Identity.Dapper.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone.Controllers
{
    
        [Authorize]
        public class UserController : Controller
        {
            private readonly UserManager<DapperIdentityUser> _userManager;
            private readonly SignInManager<DapperIdentityUser> _signInManager;
            private readonly ILogger _logger;

            public UserController(
                UserManager<DapperIdentityUser> userManager,
                SignInManager<DapperIdentityUser> signInManager,
                 ILoggerFactory loggerFactory)

            {
                _userManager = userManager;
                _signInManager = signInManager;

            }

            //
            // GET: /Account/Login
            [HttpGet]
            [AllowAnonymous]
            public IActionResult Login(string returnUrl = null)
            {
               ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

            //
            // POST: /Account/Login
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
            {
                ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/");
                if (ModelState.IsValid)
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(1, "User logged in.");
                        return RedirectToLocal(returnUrl);
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning(2, "User account locked out.");
                        return View("Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }

            //
            // GET: /Account/Register
            [HttpGet]
            [AllowAnonymous]
            public IActionResult Register(string returnUrl = null)
            {
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

            //
            // POST: /Account/Register
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
            {
                //     if not null, use this VV    if null use this VV   
                ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/");
                if (ModelState.IsValid)
                {
                    var user = new DapperIdentityUser { UserName = model.Email, Email = model.Email };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                       
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation(3, "User created a new account with password.");
                        return RedirectToLocal(returnUrl);
                    }
                    AddErrors(result);
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }

            //
            // GET: /Account/LogOff
            [HttpGet]
            // [ValidateAntiForgeryToken]
            public async Task<IActionResult> LogOff()
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation(4, "User logged out.");
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            private void AddErrors(IdentityResult result)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            private Task<DapperIdentityUser> GetCurrentUserAsync()
            {
                return _userManager.GetUserAsync(HttpContext.User);
            }

            private IActionResult RedirectToLocal(string returnUrl)
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }


        }
    }

