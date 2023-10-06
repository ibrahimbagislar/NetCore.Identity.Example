using Identity.ExampleUdemy.Data.Entites;
using Identity.ExampleUdemy.Models;
using Identity.ExampleUdemy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.ExampleUdemy.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
        }

        public IActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateUserModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Gender = model.Gender,
                    ImagePath = "/deneme.jpg"
                };

                var identityResult = await _userManager.CreateAsync(user, model.Password);
                if (identityResult.Succeeded)
                {
                    await _mailService.SendMessageAsync(model.Email, "HOŞGELDİN " + model.UserName, "IDENTITYAPP'E HOŞGELDİN " + model.UserName, true,model.UserName);
                    await _userManager.AddToRoleAsync(user, "Member");
                    await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, true);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult SignIn(string ReturnUrl)
        {
            return View(new SignInUserModel
            {
                ReturnUrl = ReturnUrl
            });
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInUserModel model)
        {

            var appUser = await _userManager.FindByNameAsync(model.UserName);
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);

                if (signInResult.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    var user = await _userManager.FindByNameAsync(model.UserName);
                    var userRole = await _userManager.GetRolesAsync(user);
                    if (userRole.Contains("Admin"))
                    {
                        return RedirectToAction("GetUserInfo", "Home");
                    }
                    return RedirectToAction("index", "home");
                }
                else if (signInResult.IsLockedOut)
                {
                    // hesap kilitli durumu
                    var lockoutEndDate = await _userManager.GetLockoutEndDateAsync(appUser);

                    ModelState.AddModelError("",$"Çok fazla yanlış deneme sonucu hesabınız {(lockoutEndDate.Value.UtcDateTime - DateTime.UtcNow).Minutes} dakika kısıtlandı.");
                    return View(model);
                }
                else if (signInResult.IsNotAllowed)
                {
                    // email yada numara doğrulanmamış olma durumu
                    var user = await _userManager.FindByNameAsync(model.UserName);

                    if (user.Email.Length > 5) // En az 6 karakterli bir e-posta adresi olmalıdır.
                    {
                        var maskedEmail = user.Email.Substring(0, 1) + new string('*', user.Email.Length - 5) + user.Email.Substring(user.Email.Length - 4);

                        ModelState.AddModelError("", $"{maskedEmail} Mail adresinizi doğrulayınız.");
                        return View(model);
                    }
                }

                else if (appUser == null)
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı");
                    return View(model);
                }
                else if (appUser != null)
                {
                    var failedCount = await _userManager.GetAccessFailedCountAsync(appUser);
                    if (appUser!=null && failedCount >= 5)
                    {
                        ModelState.AddModelError("", $"{_userManager.Options.Lockout.MaxFailedAccessAttempts - failedCount} defa daha yanlış deneme yaparsanız hesabınız 5 dakika kilitlenecektir.");
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı adı yada şifre hatalı.");
                    }
                    return View(model);
                }

            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userInfo = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(userInfo);
        }

        [Authorize(Roles = "Member")]
        public IActionResult uyepanel()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
