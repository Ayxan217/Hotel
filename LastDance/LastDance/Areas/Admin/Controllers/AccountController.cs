using LastDance.Models;
using LastDance.Utils.Enums;
using LastDance.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace LastDance.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        public IActionResult Register()
        {
           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM userVM)
        {
            if (!ModelState.IsValid)
                return View(userVM);

            //string regex = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$\r\n";
            //if (!Regex.IsMatch(userVM.Email, regex))
            //{
            //    ModelState.AddModelError(userVM.Email, "Email is not valid");
            //    return View(userVM);
            //}

            AppUser user = new()
            {
                Email = userVM.Email,
                UserName = userVM.Username,
                Name = userVM.Name,
                Surname = userVM.Surname,
            };

            IdentityResult result = await _userManager.CreateAsync(user, userVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);

                }
                return View(userVM);
            }

            await _userManager.AddToRoleAsync(user, UserRoles.MEMBER.ToString());
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM userVM, string? returnUrl)
        {
            if(!ModelState.IsValid) 
               return View(userVM);

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u=>u.Email==userVM.UsernameOrEmail || u.UserName==userVM.UsernameOrEmail);
            if (user is null)
            {
                ModelState.AddModelError(userVM.UsernameOrEmail,"user not found");
                return View(userVM);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, userVM.Password, true, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Please try again later");
                return View(userVM);
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email or Username incorrect");
                return View(userVM);
            }

            if(returnUrl is null) 
                return RedirectToAction(nameof(HomeController.Index), "Home");   

            return RedirectToAction(returnUrl);
        }


        public async Task<IActionResult> LogOut()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }


}