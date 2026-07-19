using MediatR;
using Blocks.Domain;

namespace Blocks.MediatR.Behaviours;

public class SerUserIdBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IAuditableAction
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        request.CreatedById = 1;
        return next();
    }
}
