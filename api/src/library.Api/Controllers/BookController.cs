using library.Api.Filters;
using library.Application.UseCases.BookImages.UploadImage;
using library.Application.UseCases.Books.Delete;
using library.Application.UseCases.Books.GetAll;
using library.Application.UseCases.Books.GetById;
using library.Application.UseCases.Books.GetBySlug;
using library.Application.UseCases.Books.Register;
using library.Application.UseCases.Books.Update;
using library.Communication.Requests;
using library.Communication.Responses;
using library.Communication.Responses.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace library.Api.Controllers;
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class BookController : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Gets a list of all books.")]
    [SwaggerResponse(200, "Returns the books.")]
    [SwaggerResponse(204, "No books found.")]
    [SwaggerResponse(400, "Error retrieving books.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(typeof(ApiResponse<ICollection<ResponseBookJson>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllBooksUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.Data.Count > 0)
        {
            response.StatusCode = StatusCodes.Status200OK;
            return Ok(response);
        }

        response.StatusCode = StatusCodes.Status204NoContent;
        return NoContent();
    }

    [AuthorizePermission("Admin")]
    [HttpGet]
    [Route("{id}")]
    [SwaggerOperation(Summary = "Get info a book of your id.")]
    [SwaggerResponse(200, "Returns the book.")]
    [SwaggerResponse(204, "No book found.")]
    [SwaggerResponse(400, "Error retrieving book.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    [SwaggerResponse(403, "Forbidden: You do not have permission to access this resource.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(typeof(ApiResponse<ResponseBookJson>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, [FromServices] IGetByIdBookUseCase useCase)
    {
        var result = await useCase.Execute(id);

        if(result is null)
        {
            return NoContent();
        }

        return Ok(result);
    }

    [HttpGet]
    [Route("find-by-slug/{slug}")]
    [SwaggerOperation(Summary = "Get info a book of your slug.")]
    [SwaggerResponse(200, "Returns the book.")]
    [SwaggerResponse(204, "No book found.")]
    [SwaggerResponse(400, "Error retrieving book.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(typeof(ApiResponse<ResponseBookJson>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetBySlug([FromRoute] string slug, [FromServices] IGetBySlugBookUseCase useCase)
    {
        var result = await useCase.Execute(slug);

        if (result is null)
        {
            return NoContent();
        }

        return Ok(result);
    }

    [AuthorizePermission("Admin")]
    [HttpPost]
    [SwaggerOperation(Summary = "Create a book.")]
    [SwaggerResponse(201, "Book created.")]
    [SwaggerResponse(400, "Error creating book.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    [SwaggerResponse(403, "Forbidden: You do not have permission to access this resource.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromServices] IRegisterBookUseCase useCase, [FromBody] RequestBookJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [AuthorizePermission("Admin")]
    [HttpPut]
    [Route("{id}")]
    [SwaggerOperation(Summary = "Update a book.")]
    [SwaggerResponse(204, "Book updated.")]
    [SwaggerResponse(400, "Error creating book.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    [SwaggerResponse(403, "Forbidden: You do not have permission to access this resource.")]
    [SwaggerResponse(404, "Book not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromServices] IUpdateBookUseCase useCase, [FromBody] RequestBookJson request)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }

    [AuthorizePermission("Admin")]
    [HttpDelete]
    [Route("{id}")]
    [SwaggerOperation(Summary = "Delete a book.")]
    [SwaggerResponse(204, "Book deleted.")]
    [SwaggerResponse(400, "Error deleting book.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    [SwaggerResponse(403, "Forbidden: You do not have permission to access this resource.")]
    [SwaggerResponse(404, "Book not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, [FromServices] IDeleteBookUseCase useCase)
    {
        await useCase.Execute(id);

        return NoContent();
    }

    [AuthorizePermission("Admin")]
    [HttpPost]
    [Route("upload-image/{id}")]
    [SwaggerOperation(Summary = "Upload book image.")]
    [SwaggerResponse(201, "Image uploaded.")]
    [SwaggerResponse(400, "Error deleting book.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    [SwaggerResponse(403, "Forbidden: You do not have permission to access this resource.")]
    [SwaggerResponse(404, "Book not found.")]
    [SwaggerResponse(500, "Internal server error.")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadImage([FromRoute] Guid id, [FromServices] IUploadBookImageUseCase useCase, IFormFile file)
    {
        var response = await useCase.Execute(id, file);

        return Created(string.Empty, response);
    }
}
