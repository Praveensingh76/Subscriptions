using Microsoft.AspNetCore.Mvc;
using Subscriptions.Interface;
using Subscriptions.Models;

namespace Subscriptions.Controllers
{
    public class AssessmentController : Controller
    {
        private readonly IRepository _repository;

        public AssessmentController(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var questions = await _repository.GetAllQuestionsAsync();
            var model = new QuestionnaireViewModel
            {
                Questions = questions.ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAnswers(QuestionnaireViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Assuming the responses are from the form as `model.Answers`
                foreach (var question in model.Questions)
                {
                    var response = new UserResponse
                    {
                        UserId = 1, // Replace with actual user ID
                        QuestionId = question.Id,
                        Response = "Sample answer" // Replace with the actual response from the form
                    };
                    await _repository.SaveUserResponseAsync(response);
                }

                return RedirectToAction("Recommendations", "Assessment");
            }

            return View("Index", model);
        }


        public async Task<IActionResult> Recommendations()
        {
            var userId = 1; // Replace with actual user ID
            var guidance = await _repository.GetGuidanceRecommendationsAsync(userId);

            var model = new RecommendationsViewModel
            {
                Recommendations = string.Join("<br/>", guidance.Select(g => g.GuidanceText))
            };

            return View(model);
        }
    }
}
