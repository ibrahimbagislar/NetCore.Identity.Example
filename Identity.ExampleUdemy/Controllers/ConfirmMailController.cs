using Hangfire;
using NetCore.Identity.Example.Data.Entites;
using NetCore.Identity.Example.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCore.Identity.Example.Models;
using NetCore.Identity.Example.Services;
using static System.Net.WebRequestMethods;

namespace NetCore.Identity.Example.Controllers
{
    [Authorize]
    public class ConfirmMailController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMailService _mailService;

        public ConfirmMailController(UserManager<AppUser> userManager, IMailService mailService)
        {
            _userManager = userManager;
            _mailService = mailService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.Email = user.Email;
                ViewBag.Id = user.Id;
                var checkConfirmMail = await _userManager.IsEmailConfirmedAsync(user);
                if (!checkConfirmMail && (user.ConfirmCodeEndDate - DateTime.UtcNow).TotalSeconds < 0)
                    ModelState.AddModelError("", "Yeni doğrulama kodun " + user.Email + " adresine gönderildi.");
                return View();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ConfirmMailViewModel model)
        {
            ViewBag.Email = model.Email;
            ViewBag.Id = model.Id;

            var user = await _userManager.FindByEmailAsync(model.Email);
            var checkEndDate = user.ConfirmCodeEndDate - DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(model.Email) && checkEndDate.TotalSeconds > 0)
            {
                if (user.ConfirmCode == model.ConfirmCode)
                {
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction("Index", "Profile");
                }
                ModelState.AddModelError("", "Hatalı doğrulama kodu.");
            }
            else if (checkEndDate.TotalSeconds < 0)
            {
                ModelState.AddModelError("", "Yeni doğrulama kodun " + user.Email + " adresine gönderildi.");
                SendConfirmMail mail = new(_mailService, _userManager);
                await mail.SendAsync(user);
            }
            return View(model);
        }
    }
}
