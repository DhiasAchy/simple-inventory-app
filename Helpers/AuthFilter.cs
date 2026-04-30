using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SimpleInventoryApp.Helpers;

namespace SimpleInventoryApp.Helpers
{
    public class AuthFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var path = context.HttpContext.Request.Path.Value;

            // ✅ IZINKAN AKSES KE LOGIN
            if (path != null &&
                (path.ToLower().Contains("/auth/login") ||
                path.ToLower().Contains("/auth/logout") ||
                path.ToLower().Contains("/auth/register")))
            {
                return;
            }

            var user = context.HttpContext.Session.GetString(SessionKey.Username);

            if (user == null)
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}