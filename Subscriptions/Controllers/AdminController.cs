using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Subscriptions.Interface;
using Subscriptions.Models;
using Subscriptions.Utility;

namespace Subscriptions.Controllers
{
    [RoleAuthorizationFilter]
    public class AdminController : Controller
    {

        private readonly IRepository _repository;

        public AdminController(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Index()
        {
            var adminView = await _repository.GetAdminAsync();
            return View(adminView);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            if (login.Email == "admin@gmail.com" && login.Password == "12345678")
            {
                HttpContext.Session.SetString("Id", AppCode.encrypt("1"));
                HttpContext.Session.SetString("Role", AppCode.encrypt("Admin"));
                TempData["SuccessMessage"] = "Login successful. Welcome back!";
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid email or password.";
                return View();
            }

        }


        public async Task<IActionResult> UserManagement()
        {
            var users = await _repository.GetAllUsersAsync();
            return View(users);
        }

        public async Task<IActionResult> QuestionManagement()
        {
            var questions = await _repository.GetAllQuestionsAsync();
            return View(questions);
        }

        public async Task<IActionResult> GuidanceManagement()
        {
            var guidance = await _repository.GetAllGuidanceAsync();
            return View(guidance);
        }

        public async Task<IActionResult> ViewResponses()
        {
            var responses = await _repository.GetAllResponsesAsync();
            return View(responses);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Admin");
        }

    }
}
