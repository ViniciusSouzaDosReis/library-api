namespace library.Communication.Responses;

public class ResponseErrorJson
{
    public List<string> ErrorMessage { get; set; }

    public ResponseErrorJson(string message)
    {
        ErrorMessage = [message];
    }

    public ResponseErrorJson(List<string> message)
    {
        ErrorMessage = message;
    }
}
