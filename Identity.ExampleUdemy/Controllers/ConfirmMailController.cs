using Microsoft.AspNetCore.Mvc;

namespace NetCore.Identity.Example.Controllers
{
    public class ConfirmMailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
