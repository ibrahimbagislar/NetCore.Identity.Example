using NetCore.Identity.Example.Data.Contexts;
using NetCore.Identity.Example.Data.Entites;
using NetCore.Identity.Example.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace NetCore.Identity.Example.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }
        public IActionResult Create()
        {
            return View(new CreateUserAdminModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserAdminModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.UserName,
                    Gender = model.Gender,
                    Email = model.Email,
                    ImagePath = "/deneme.jpg"
                };
                var result = await _userManager.CreateAsync(user, model.UserName + "123");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Member");
                    return RedirectToAction("Index", "User");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole(int id)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.ToList();

            RoleAssignSendModel model = new();
            List<RoleAssignListModel> list = new();

            foreach (var role in roles)
            {
                list.Add(new()
                {
                    Name = role.Name,
                    RoleId = role.Id,
                    Exist = userRoles.Contains(role.Name)
                });
            }

            model.Roles = list;
            model.UserId = id;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(RoleAssignSendModel model)
        {
            // iki durum var seçilen rollerin eklenmesi ya da seçilmeyen rollerin çıkarılması
            // ekleme durumunda seçilen rolün kullanıcı da zaten var olmaması gerekli
            // çıkarma durumunda ise seçilen rolün kullanıcı da olması gerekli bu iki durumu kontrol edip işlemleri gerçekleştireceğiz.

            var user = _userManager.Users.SingleOrDefault(x => x.Id == model.UserId);
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in model.Roles)
            {
                if (role.Exist)
                {
                    if (!userRoles.Contains(role.Name))
                        await _userManager.AddToRoleAsync(user, role.Name);
                }
                else
                {
                    if (userRoles.Contains(role.Name))
                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }

            return RedirectToAction("Index", "User");
        }
    }
}
