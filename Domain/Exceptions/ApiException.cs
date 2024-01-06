using System.Net;

namespace Domain.Exceptions;

public class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public ApiException(HttpStatusCode statusCode, string message = default) : base(message)
    {
        StatusCode = statusCode;
    }
}