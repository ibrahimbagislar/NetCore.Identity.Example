using Identity.ExampleUdemy.Data.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCore.Identity.Example.Models;

namespace NetCore.Identity.Example.Controllers
{
    public class ConfirmMailController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ConfirmMailController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Email = TempData["Mail"];
            var user = await _userManager.FindByEmailAsync(ViewBag.Email);
            ViewBag.Id = user.Id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ConfirmMailViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user.ConfirmCode == model.ConfirmCode)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Index","Profile");
            }
            return View(model);
        }
    }
}
