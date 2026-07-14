using System.Net;

namespace Blocks.Exceptions;

public class HttpException : Exception
{
    public HttpException(HttpStatusCode statusCode, string message) 
        : base(string.IsNullOrEmpty(message) ? statusCode.ToString() : message)
    {
        this.HttpStatusCode = statusCode;
    }
    public HttpException(HttpStatusCode statusCode, string message, Exception innerException)
        : base(string.IsNullOrEmpty(message) ? statusCode.ToString() : message, innerException)
    {
        this.HttpStatusCode = statusCode;
    }
    public HttpStatusCode HttpStatusCode { get; }
}
