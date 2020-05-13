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
using Capstone.Models.UserViewModels;

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
            _logger = loggerFactory.CreateLogger<UserController>();

            }

  
            //
            // GET: /User/Login
            [HttpGet]
            [AllowAnonymous]
            public IActionResult Login(string returnUrl = null)
            {
               ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

            //
            // POST: /User/Login
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
            {
                ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/");
                if (ModelState.IsValid)
                {
                // This doesn't count login failures towards User lockout
                // To enable password failures to trigger User lockout, set lockoutOnFailure: true
                
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(1, "User logged in.");
                        return RedirectToLocal(returnUrl);
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning(2, "User User locked out.");
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
            // GET: /User/Register
            [HttpGet]
            [AllowAnonymous]
            public IActionResult Register(string returnUrl = null)
            {
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

            //
            // POST: /User/Register
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
                if (!result.Succeeded)
                {
                    var exceptionText = string.Join(", ", result.Errors.Select(x => "Code " + x.Code + " Description" + x.Description));
                    throw new Exception(exceptionText);

                }
                //AddErrors(result);

            }

                // If we got this far, something failed, redisplay form
                return View(model);
            }

            //
            // GET: /User/LogOff
            [HttpPost]
            [AllowAnonymous]
            // [ValidateAntiForgeryToken]
            public async Task<IActionResult> LogOff()
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation(4, "User logged out.");
                return RedirectToAction("Index", "Home");
            }

            private void AddErrors(IdentityResult result)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

        // GET: /User/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }

        //
        // GET: /User/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /User/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View(nameof(ForgotPasswordConfirmation));
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /User/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /User/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /User/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(UserController.ResetPasswordConfirmation), "User");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(UserController.ResetPasswordConfirmation), "User");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /User/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
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

