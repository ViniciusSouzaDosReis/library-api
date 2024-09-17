using library.Api.Filters;
using library.Application.UseCases.Reservations.Delete;
using library.Application.UseCases.Reservations.GetAll;
using library.Application.UseCases.Reservations.Pick;
using library.Application.UseCases.Reservations.Register;
using library.Application.UseCases.Reservations.Request;
using library.Application.UseCases.Reservations.Return;
using library.Communication.Responses;
using library.Communication.Responses.Reservations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace library.Api.Controllers;
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ReservationController : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Gets a list of all reservations.")]
    [SwaggerResponse(200, "Returns the reservations.")]
    [SwaggerResponse(204, "No reservations found.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(typeof(ApiResponse<ICollection<ResponseReservationJson>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllReservationsUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.Data.Count > 0)
            return NoContent();

        return Ok(response);
    }

    [HttpPost]
    [Route("{bookId}")]
    [SwaggerOperation(Summary = "Creates a reservation for the book.")]
    [SwaggerResponse(201, "Reservation created.")]
    [SwaggerResponse(400, "Error creating reservation.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    [SwaggerResponse(404, "Book not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(Guid bookId, [FromServices] IRegisterReservationUseCase useCase)
    {
        var response = await useCase.Execute(bookId);

        return Created(string.Empty, response);
    }

    [AuthorizePermission("Admin")]
    [HttpDelete]
    [Route("{id}")]
    [SwaggerOperation(Summary = "Delete reservation.")]
    [SwaggerResponse(204, "Reservation deleted.")]
    [SwaggerResponse(400, "Error deleting reservation.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    [SwaggerResponse(403, "Forbidden: You do not have permission to access this resource.")]
    [SwaggerResponse(404, "Reservation not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, [FromServices] IDeleteReservationUseCase useCase)
    {
        await useCase.Execute(id);

        return NoContent();
    }

    [AuthorizePermission("Admin")]
    [HttpPut]
    [Route("{id}/returned")]
    [SwaggerOperation(Summary = "Change reservation status to returned.")]
    [SwaggerResponse(204, "Changeded status.")]
    [SwaggerResponse(400, "Error change status.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    [SwaggerResponse(403, "Forbidden: You do not have permission to access this resource.")]
    [SwaggerResponse(404, "Reservation not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Return(Guid id, [FromServices] IReturnReservationUseCase useCase)
    {
        await useCase.Execute(id);

        return NoContent();
    }

    [AuthorizePermission("Admin")]
    [HttpPut]
    [Route("{id}/pickup")]
    [SwaggerOperation(Summary = "Change reservation status to in use.")]
    [SwaggerResponse(204, "Changeded status.")]
    [SwaggerResponse(400, "Error change status.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    [SwaggerResponse(403, "Forbidden: You do not have permission to access this resource.")]
    [SwaggerResponse(404, "Reservation not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Pick(Guid id, [FromServices] IPickReservationUseCase useCase)
    {
        await useCase.Execute(id);

        return NoContent();
    }

    [AuthorizePermission("Admin")]
    [HttpPut]
    [Route("{id}/request")]
    [SwaggerOperation(Summary = "Change reservation status to requested.")]
    [SwaggerResponse(204, "Changeded status.")]
    [SwaggerResponse(400, "Error change status.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    [SwaggerResponse(403, "Forbidden: You do not have permission to access this resource.")]
    [SwaggerResponse(404, "Reservation not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Requested(Guid id, [FromServices] IRequestBookUseCase useCase)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}
