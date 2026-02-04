using Microsoft.AspNetCore.Mvc;
using Subscriptions.Interface;

namespace Subscriptions.Controllers
{
    public class AdminController : Controller
    {

        private readonly IRepository _repository;

        public AdminController(IRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            return View();
        }


        //public async Task<IActionResult> UserManagement()
        //{
        //    var users = await _repository.GetAllUsersAsync();
        //    return View(users);
        //}

        //public async Task<IActionResult> QuestionManagement()
        //{
        //    var questions = await _repository.GetAllQuestionsAsync();
        //    return View(questions);
        //}

        //public async Task<IActionResult> GuidanceManagement()
        //{
        //    var guidance = await _repository.GetAllGuidanceAsync();
        //    return View(guidance);
        //}

        //public async Task<IActionResult> ViewResponses()
        //{
        //    var responses = await _repository.GetAllResponsesAsync();
        //    return View(responses);
        //}

    }
}
