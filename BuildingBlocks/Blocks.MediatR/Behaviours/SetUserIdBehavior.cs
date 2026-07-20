using MediatR;
using Blocks.Domain;

namespace Blocks.MediatR.Behaviours;

public class SetUserIdBehavior<TRequest, TResponse>(ICurrentUser currentUser) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IAuditableAction
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        request.CreatedById = currentUser.UserId;
        return next();
    }
}
