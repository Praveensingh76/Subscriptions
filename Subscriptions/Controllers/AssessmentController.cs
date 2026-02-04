using Microsoft.AspNetCore.Mvc;
using Subscriptions.Interface;
using Subscriptions.Models;
using Subscriptions.Utility;

namespace Subscriptions.Controllers
{
    [RoleAuthorizationFilter]
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
                var userIdEncrypted = HttpContext.Session.GetString("Id");
                var userId = int.Parse(AppCode.Decrypt(userIdEncrypted));
                // Assuming the responses are from the form as `model.Answers`
                foreach (var question in model.Questions)
                {
                    var response = new UserResponse
                    {

                        UserId = userId,
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
            var userIdEncrypted = HttpContext.Session.GetString("Id");
            var userId = int.Parse(AppCode.Decrypt(userIdEncrypted));
            var guidance = await _repository.GetGuidanceRecommendationsAsync(userId);

            var model = new RecommendationsViewModel
            {
                Recommendations = string.Join("<br/>", guidance.Select(g => g.GuidanceText))
            };

            return View(model);
        }
    }
}
