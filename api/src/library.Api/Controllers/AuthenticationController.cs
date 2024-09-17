using library.Application.UseCases.Tokens.Generate;
using library.Application.UseCases.Tokens.Logout;
using library.Communication.Requests;
using library.Communication.Responses;
using library.Communication.Responses.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace library.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    [SwaggerOperation(Summary = "Logs in a user and returns a JWT.")]
    [SwaggerResponse(200, "Token generated successfully.")]
    [SwaggerResponse(400, "Invalid request or authentication failure.")]
    [SwaggerResponse(404, "User not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(typeof(ApiResponse<ResponseLoginJson>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<dynamic>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] RequestLoginJson request, [FromServices] IGenerateTokenUseCase useCase)
    {
        var response = await useCase.Execute(request);

        if (response.Data is null)
        {
            return BadRequest(response);
        }
            
        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    [Route("logout")]
    [SwaggerOperation(Summary = "Logs out a user, invalidating their token.")]
    [SwaggerResponse(200, "User successfully logged out.")]
    [SwaggerResponse(400, "Failed to log out user.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    [SwaggerResponse(404, "User not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<dynamic>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout([FromBody] RequestLogoutJson request, [FromServices] ILogoutTokenUseCase useCase)
    {
        var result = await useCase.Execute(request);

        return Ok(result);
    }
}
