namespace library.Exception.ExceptionBase;

public abstract class LibraryException : SystemException
{
    protected LibraryException(string message) : base(message) { }
    public abstract int StatusCode { get; }
    public abstract List<string> GetErrors();
}
