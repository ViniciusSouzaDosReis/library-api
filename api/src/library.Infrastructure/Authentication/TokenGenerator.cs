using library.Domain.Entities;
using library.Domain.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using library.Domain.Configurations;

namespace library.Infrastructure.Authentication;

public class TokenGenerator : ITokenGenerator
{
    private readonly IJwtConfiguration _jwtConfiguration;

    public TokenGenerator(IJwtConfiguration jwtConfiguration)
    {
        _jwtConfiguration = jwtConfiguration;
    }

    public string Generator(User user)
    {
        var expires = DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpirationInMinutes);

        var claims = new List<Claim>
        {
            new Claim("Name", user.FirstName),
            new Claim("Email", user.Email),
            new Claim("Role", user.Role.ToString()),
            new Claim("Id", user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfiguration.Secret));

        var tokenData = new JwtSecurityToken(
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
            expires: expires,
            claims: claims
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenData);
    }
}
