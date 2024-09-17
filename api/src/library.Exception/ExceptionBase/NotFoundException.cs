using System.Net;

namespace library.Exception.ExceptionBase;

public class NotFoundException : LibraryException
{
    public NotFoundException(string message) : base(message) { }

    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
