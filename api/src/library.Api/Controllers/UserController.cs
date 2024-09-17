using library.Api.Filters;
using library.Application.UseCases.Users.Delete;
using library.Application.UseCases.Users.GetAll;
using library.Application.UseCases.Users.Register;
using library.Communication.Requests;
using library.Communication.Responses;
using library.Communication.Responses.User;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace library.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[AuthorizePermission("Admin")]
public class UserController : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get a list of all users.")]
    [SwaggerResponse(200, "Returns the users.")]
    [SwaggerResponse(204, "No books found.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    [SwaggerResponse(403, "Forbidden: You do not have permission to access this resource.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(typeof(ApiResponse<ICollection<ResponseUserJson>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllUsersUseCase useCase)
    {
        var result = await useCase.Execute();

        if (result.Data.Count == 0)
        {
            return NoContent();
        }

        return Ok(result);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a user.")]
    [SwaggerResponse(201, "User created.")]
    [SwaggerResponse(400, "Error creating user.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    [SwaggerResponse(403, "Forbidden: You do not have permission to access this resource.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post( [FromServices] IRegisterUserUseCase useCase, [FromBody] RequestUserJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpDelete]
    [Route("{id}")]
    [SwaggerOperation(Summary = "Delete a user.")]
    [SwaggerResponse(201, "User deleted.")]
    [SwaggerResponse(400, "Error deleting user.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    [SwaggerResponse(403, "Forbidden: You do not have permission to access this resource.")]
    [SwaggerResponse(404, "User not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromServices] IDeleteUserUseCase useCase, Guid id)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}
