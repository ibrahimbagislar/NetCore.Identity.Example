using Identity.ExampleUdemy.Data.Entites;
using Identity.ExampleUdemy.Models;
using Identity.ExampleUdemy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCore.Identity.Example.Services;

namespace Identity.ExampleUdemy.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly RoleManager<AppRole> _roleManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var appUser = await _userManager.FindByNameAsync(User.Identity.Name);
                var checkConfirmMail = await _userManager.IsEmailConfirmedAsync(appUser);
                if (!checkConfirmMail)
                    return RedirectToAction("Index", "ConfirmMail");
            }
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

                Random rnd = new();
                AppUser user = new()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Gender = model.Gender,
                    ImagePath = "/deneme.jpg",
                    ConfirmCode = rnd.Next(100000, 1000000),
                    ConfirmCodeEndDate = DateTime.UtcNow.AddMinutes(3)
                };
                var identityResult = await _userManager.CreateAsync(user, model.Password);
                if (identityResult.Succeeded)
                {
                    SendConfirmMail mail = new(_mailService,_userManager);
                    await mail.SendAsync(user);
                    var addRoleResult = await _roleManager.FindByNameAsync("Member");
                    if (addRoleResult == null)
                    {
                        var role = new AppRole
                        {
                            Name = "Member",
                            CreatedTime = DateTime.Now
                        };
                        await _roleManager.CreateAsync(role);
                        await _userManager.AddToRoleAsync(user, "Member");
                    }
                    await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, true);

                    return RedirectToAction("Index", "ConfirmMail");
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

                    ModelState.AddModelError("", $"Çok fazla yanlış deneme sonucu hesabınız {(lockoutEndDate.Value.UtcDateTime - DateTime.UtcNow).Minutes} dakika kısıtlandı.");
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
                    if (appUser != null && failedCount >= 5)
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
