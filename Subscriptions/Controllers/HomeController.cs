using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Subscriptions.Interface;
using Subscriptions.Models;

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
                var user = await _repo.LoginUserAsync(model.Email, model.Password);
                if (user != null)
                {
                    // Add authentication logic here (e.g., set a cookie or JWT token)
                    return RedirectToAction("Index", "Admin");  // Redirect to home or dashboard
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            user.SubscriptionStatus = "Pending";


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
