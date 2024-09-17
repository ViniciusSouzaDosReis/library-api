namespace library.Communication.Responses;

public class ApiResponse
{
    public bool Success { get; set; }
    //public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    //public ICollection<ErrorValidation>? Errors { get; set; }

    public static ApiResponse<T> CreateSuccesResponseWithData<T>(T data, int statusCode)
    {
        return new ApiResponse<T>
        {
            Success = true,
            StatusCode = statusCode,
            Data = data
        };
    }

    public static ApiResponse CreateSuccesResponse(int statusCode)
    {
        return new ApiResponse
        {
            Success = true,
            StatusCode = statusCode
        };
    }
}
public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }
}

public class ApiResponse<T, TError> : ApiResponse<T>
{
    public TError? Errors { get; set; }
}
