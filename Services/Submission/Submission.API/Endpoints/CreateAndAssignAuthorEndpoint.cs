using Article.Security;
using Articles.Abstractions.Enums;
using MediatR;
using Submission.Application.Features.CreateAndAssignAuthor;

namespace Submission.API.Endpoints;

public static class CreateAndAssignAuthorEndpoint
{
    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/articles/{articleId:int}/authors",
            async (int articleId, CreateAndAssignAuthorCommand command, ISender sender) =>
        {
            var response = await sender.Send(command with { ArticleId = articleId });
            return Results.Ok(response);
        })
         .RequireRoleAuthorization(Roles.CORAUT)
        .WithName("CreateAndAssignAuthor")
        .WithTags("Submission")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden);
    }
}
