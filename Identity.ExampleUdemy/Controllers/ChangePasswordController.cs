using Identity.ExampleUdemy.Data.Entites;
using Identity.ExampleUdemy.Models;
using Identity.ExampleUdemy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.ExampleUdemy.Controllers
{
    [Authorize]
    public class ChangePasswordController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMailService _mailService;
        public ChangePasswordController(UserManager<AppUser> userManager, IMailService mailService)
        {
            _userManager = userManager;
            _mailService = mailService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var appUser = await _userManager.FindByNameAsync(User.Identity.Name);
                var checkConfirmMail = await _userManager.IsEmailConfirmedAsync(appUser);
                if (!checkConfirmMail)
                    return RedirectToAction("Index", "ConfirmMail");
            }
            return View(new ChangePasswordModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(ChangePasswordModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var checkPassword = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (model.IsCurrentPasswordMatch() == false)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else if (model.IsCurrentPasswordMatch() == true && checkPassword == true)
            {
                ModelState.AddModelError("", "Mevcut şifre ile yeni şifre aynı olamaz.");
            }
            else
            {
                ModelState.AddModelError("", "Mevcut şifre yanlış.");
            }
            return View(model);
        }
    }
}
