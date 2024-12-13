using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AuthorizeUserAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userIdClaim = user.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            context.Result = new ForbidResult();
            return;
        }

        var routeUserId = context.RouteData.Values["userId"]?.ToString();
        if (string.IsNullOrEmpty(routeUserId) || routeUserId != userIdClaim)
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}
