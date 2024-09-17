using library.Domain.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace library.Infrastructure.Authentication;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetId()
    {
        string id = GetClaimValue("Id");
        return Guid.Parse(id);
    }

    public string GetRole()
    {
        return GetClaimValue("Role");
    }

    private string GetClaimValue(string claimType)
    {
        var httpContext = _httpContextAccessor.HttpContext ?? throw new Exception("HttpContext not found");

        ClaimsIdentity? identity = httpContext.User.Identity as ClaimsIdentity ?? throw new Exception("User not found");
        var claim = identity.FindFirst(claimType) ?? throw new Exception($"Claim {claimType} not found");

        return claim.Value;
    }
}
