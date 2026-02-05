using Microsoft.AspNetCore.Mvc;
using Subscriptions.Interface;
using Subscriptions.Models;
using Subscriptions.Utility;

namespace Subscriptions.Controllers
{
    [RoleAuthorizationFilter]
    public class DashboardController : Controller
    {
        private readonly IRepository _repository;

        public DashboardController(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Index()
        {
            var userIdEncrypted = HttpContext.Session.GetString("Id");
            var userId = int.Parse(AppCode.Decrypt(userIdEncrypted));
            var subscription = await _repository.CheckSubscriptionAsync(userId);
            var responses = await _repository.GetUserResponsesAsync(userId);
            var question = await _repository.GetAllQuestionsAsync();

            var model = new UserDashboardViewModel
            {
                Subscription = subscription,
                CompletedQuestionsCount = responses.Count(),
                TotalQuestionsCount = question.Count(),
                Guidance = "Sample recommendation based on your answers" // Get this from the DB if needed
            };

            return View(model);
        }
    }
}
