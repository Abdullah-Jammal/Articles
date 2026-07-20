using System.Net;

namespace Blocks.Exceptions;

public sealed class ForbiddenException(string message)
    : HttpException(HttpStatusCode.Forbidden, message);
