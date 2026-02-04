using Microsoft.AspNetCore.Mvc;
using Subscriptions.Interface;
using Subscriptions.Models;

namespace Subscriptions.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IRepository _repository;

        public DashboardController(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Index()
        {
            var userId = 1; // Replace this with actual logged-in user's ID.
            var subscription = await _repository.CheckSubscriptionAsync(userId);
            var responses = await _repository.GetUserResponsesAsync(userId);

            var model = new UserDashboardViewModel
            {
                Subscription = subscription,
                CompletedQuestionsCount = responses.Count(),
                TotalQuestionsCount = 20, // Adjust based on actual question count
                Guidance = "Sample recommendation based on your answers" // Get this from the DB if needed
            };

            return View(model);
        }
    }
}
