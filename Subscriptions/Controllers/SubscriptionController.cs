using Microsoft.AspNetCore.Mvc;
using Subscriptions.Interface;
using Subscriptions.Models;
using Subscriptions.Utility;

namespace Subscriptions.Controllers
{
    [RoleAuthorizationFilter]
    public class SubscriptionController : Controller
    {
        private readonly IRepository _repository;

        public SubscriptionController(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Manage()
        {
            var userIdEncrypted = HttpContext.Session.GetString("Id");
            var userId = int.Parse(AppCode.Decrypt(userIdEncrypted));
            var subscription = await _repository.CheckSubscriptionAsync(userId);

            if (subscription == null)
            {
                return RedirectToAction("Index", "Dashboard"); // Redirect if no subscription exists
            }

            var model = new SubscriptionManagementViewModel
            {
                Subscription = subscription,
                AvailablePlans = new[] { "1 Month", "2 Months" },
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RenewSubscription(string planType)
        {
            var userIdEncrypted = HttpContext.Session.GetString("Id");
            var userId = int.Parse(AppCode.Decrypt(userIdEncrypted));
            var subscription = new Subscription
            {
                UserId = userId,
                PlanType = planType,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(planType == "1 Month" ? 1 : 2)
            };

            await _repository.CreateOrUpdateSubscriptionAsync(subscription);

            return RedirectToAction("Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelSubscription()
        {
            var userIdEncrypted = HttpContext.Session.GetString("Id");
            var userId = int.Parse(AppCode.Decrypt(userIdEncrypted));
            var subscription = await _repository.CheckSubscriptionAsync(userId);

            if (subscription != null)
            {
                subscription.IsActive = false;
                await _repository.CreateOrUpdateSubscriptionAsync(subscription);
            }

            return RedirectToAction("Manage");
        }
    }
}
