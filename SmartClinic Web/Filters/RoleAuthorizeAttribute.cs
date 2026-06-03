using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SmartClinic.Web.Filters
{
    public class RoleAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string[] _roles;

        public RoleAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("Role");           

            var token = context.HttpContext.Session.GetString("JWToken");

            // Session expired or not logged in
            if (string.IsNullOrEmpty(token))
            {
                context.Result =
                    new RedirectToActionResult(
                        "Login",
                        "Account",
                        null);

                return;
            }

            // Role check
            if (!_roles.Contains(role))
            {
                context.Result =
                    new RedirectToActionResult(
                        "AccessDenied",
                        "Account",
                        null);
            }
        }
    }
}
