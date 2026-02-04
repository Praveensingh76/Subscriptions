using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Subscriptions.Interface;
using Subscriptions.Models;
using Subscriptions.Utility;

namespace Subscriptions.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repo;
        public HomeController(ILogger<HomeController> logger, IRepository repository)
        {
            _logger = logger;
            _repo = repository;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _repo.LoginUserAsync(model.Email, AppCode.encrypt(model.Password));
                if (user != null)
                {
                    HttpContext.Session.SetString("Id", AppCode.encrypt(user.Id.ToString()));
                    HttpContext.Session.SetString("Role", AppCode.encrypt("User"));
                    return RedirectToAction("Index", "Dashboard");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            user.PasswordHash = AppCode.encrypt(user.PasswordHash);
            user.SubscriptionStatus = "Active";


            var result = await _repo.CreateUser(user);
            if (result != 0)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Email is already registered");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
