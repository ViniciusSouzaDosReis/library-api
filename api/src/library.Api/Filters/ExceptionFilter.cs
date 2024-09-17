using library.Communication.Responses;
using library.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace library.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is LibraryException)
        {
            HandleProjectException(context);
        }
        else
        {
            ThrowUnknownError(context);
        }
    }

    private void HandleProjectException(ExceptionContext context)
    {
        var exception = (LibraryException)context.Exception;
        context.HttpContext.Response.StatusCode = exception.StatusCode;

        var statusCode = exception.StatusCode;
        var response = new ApiResponse<string, List<string>>
        {
            Success = false,
            StatusCode = exception.StatusCode,
            Data = null,
            Errors = exception.GetErrors()
        };

        context.Result = new ObjectResult(response);
    }

    private void ThrowUnknownError(ExceptionContext context)
    {
        var errorMessage = new ApiResponse<string, List<string>>
        {
            Success = false,
            StatusCode = StatusCodes.Status500InternalServerError,
            Data = null,
            Errors = new List<string> { "Unknown error" }
        };

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorMessage);
    }
}
