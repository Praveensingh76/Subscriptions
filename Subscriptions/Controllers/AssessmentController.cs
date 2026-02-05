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
            if (model == null || model.Answers == null || !model.Answers.Any())
            {
                ModelState.AddModelError("", "Please answer at least one question.");
                return View("Index", model);
            }

            var userIdEncrypted = HttpContext.Session.GetString("Id");
            var userId = int.Parse(AppCode.Decrypt(userIdEncrypted));

            foreach (var answer in model.Answers)
            {
                var response = new UserResponse
                {
                    UserId = userId,
                    QuestionId = answer.Key,
                    Response = answer.Value
                };

                await _repository.SaveUserResponseAsync(response);
            }

            return RedirectToAction("Recommendations", "Assessment");
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
