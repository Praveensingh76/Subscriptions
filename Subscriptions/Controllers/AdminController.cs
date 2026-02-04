using Microsoft.AspNetCore.Mvc;

namespace Subscriptions.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
