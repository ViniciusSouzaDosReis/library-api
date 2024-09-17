using library.Communication.Responses;
using library.Exception.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace library.Api.Filters;

public class AuthorizePermission : Attribute, IAuthorizationFilter
{
    private readonly string _role;

    public AuthorizePermission(string role)
    {
        _role = role;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new ObjectResult(new ApiResponse<string, List<string>>
            {
                Success = false,
                StatusCode = StatusCodes.Status401Unauthorized,
                Data = null,
                Errors = new List<string> { ResourceErrorMessage.USER_NOT_AUTHENTICATED }
            })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
            return;
        }

        if (!user.Claims.Any(c => c.Type == "Role" && c.Value.ToString().Equals("Admin")))
        {
            context.Result = new ObjectResult(new ApiResponse<string, List<string>>
            {
                Success = false,
                StatusCode = StatusCodes.Status403Forbidden,
                Data = null,
                Errors = new List<string> { ResourceErrorMessage.USER_DOES_NOT_HAVE_PERMISSION }
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }
    }
}
