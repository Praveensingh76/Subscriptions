using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Subscriptions.Utility
{
    public class RoleAuthorizationFilter : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var httpContext = context.HttpContext;


            var allowAnonymous = context.ActionDescriptor.EndpointMetadata
                .OfType<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>()
                .Any();

            if (allowAnonymous)
            {
                await next();
                return;
            }

            var encryptedUserId = httpContext.Session.GetString("Id");
            var encryptedRole = httpContext.Session.GetString("Role");


            if (string.IsNullOrEmpty(encryptedUserId) || string.IsNullOrEmpty(encryptedRole))
            {
                context.Result = RedirectTo("Home", "Index");
                return;
            }

            string role;
            try
            {
                role = AppCode.Decrypt(encryptedRole);
            }
            catch
            {
                context.Result = RedirectTo("Home", "Index");
                return;
            }

            var controllerName = context.RouteData.Values["controller"]?.ToString();


            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                if (controllerName != "Admin")
                {
                    context.Result = RedirectTo("Admin", "Index");
                    return;
                }
            }
            else if (role.Equals("User", StringComparison.OrdinalIgnoreCase))
            {
                if (controllerName != "Home"
                    && controllerName != "Dashboard"
                    && controllerName != "Assessment"
                    && controllerName != "Subscription")
                {
                    context.Result = RedirectTo("Home", "Index");
                    return;
                }
            }
            else
            {
                context.Result = RedirectTo("Home", "Index");
                return;
            }

            await next();
        }

        private RedirectToRouteResult RedirectTo(string controller, string action)
        {
            return new RedirectToRouteResult(new RouteValueDictionary
            {
                { "controller", controller },
                { "action", action }
            });
        }
    }
}
