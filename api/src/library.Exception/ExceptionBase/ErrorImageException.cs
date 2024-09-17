using System.Net;

namespace library.Exception.ExceptionBase;

public class ErrorImageException : LibraryException
{
    public ErrorImageException(string message) : base(message) { }

    public override int StatusCode => (int)HttpStatusCode.InternalServerError;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
