using Identity.ExampleUdemy.Data.Entites;
using Identity.ExampleUdemy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.ExampleUdemy.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userInfo = await _userManager.GetUserAsync(User);
            var user = new ProfileInfoModel
            {
                Email = userInfo.Email,
                UserName = userInfo.UserName,
                Gender = userInfo.Gender,
            };
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Index(ProfileInfoModel model)
        {
            var userInfo = await _userManager.GetUserAsync(User);

            userInfo.Email = model.Email;
            userInfo.UserName = model.UserName;
            userInfo.Gender = model.Gender;

            var result = await _userManager.UpdateAsync(userInfo);


            if(result.Succeeded)
            {
                return RedirectToAction("Index","Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
                return View(model);
            }
            return View(model);
        }
    }
}
