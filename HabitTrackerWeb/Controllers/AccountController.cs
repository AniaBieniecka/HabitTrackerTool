using Azure.Identity;
using HabitTracker.DataAccess.Repository.IRepository;
using HabitTracker.Models;
using HabitTracker.Models.ScoringModels;
using HabitTracker.Models.ViewModels;
using HabitTrackerWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HabitTrackerWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Login(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            LoginVM loginVM = new()
            {
                RedirectUrl = returnUrl
            };

            return View(loginVM);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, loginVM.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(loginVM.UserName);
                    if (await _userManager.IsInRoleAsync(user, SD.Role_Admin))
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(loginVM.RedirectUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return LocalRedirect(loginVM.RedirectUrl);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            return View(loginVM);
        }
        public IActionResult Register(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).Wait();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).Wait();
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            var users = await _userManager.Users.ToListAsync();

            var userFromDb = users.FirstOrDefault(u=>u.NormalizedEmail==registerVM.UserName.ToUpper());

            if(userFromDb != null)
            {
              ModelState.AddModelError("UserName", "User with this name already exist");
            }

            if (ModelState.IsValid)
            {
                IdentityUser user = new()
                {
                    UserName = registerVM.UserName,
                    NormalizedUserName = registerVM.UserName.ToUpper(),
                    Email = registerVM.Email,
                    NormalizedEmail = registerVM.Email.ToUpper(),
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, registerVM.Password);

                var viewSetting = new ViewSetting()
                {
                    Color = "00CED1",
                    IconDone = "bi bi-check-square-fill",
                    IconPartiallyDone = "bi bi-check-square",
                    UserId = user.Id,
                };

                _unitOfWork.ViewSetting.Add(viewSetting);
                _unitOfWork.Save();

                var score = new Score() 
                { 
                    ScoreValue = 0,
                    LevelId = 0,
                    UserId = user.Id

                };
                _unitOfWork.Score.Update(score);
                _unitOfWork.Save();

                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                    

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    if (string.IsNullOrEmpty(registerVM.RedirectUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return LocalRedirect(registerVM.RedirectUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            return View(registerVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
