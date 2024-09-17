using System.Security.Claims;

public static class JwtHelpers
{
    public static string GetClaimValue(HttpContext httpContext, string claimType)
    {
        var identity = httpContext.User.Identity as ClaimsIdentity;
        return identity.FindFirst(claimType)?.Value;
    }

    public static string GetUserId(HttpContext httpContext)
    {
        return GetClaimValue(httpContext, "Id");
    }

    public static string GetUserRole(HttpContext httpContext)
    {
        return GetClaimValue(httpContext, "Role");
    }
}
