using MediatR;
using Articles.Abstractions.Enums;

namespace Submission.API.Endpoints;

using Article.Security;
using Submission.Application.Features.AssignAuthor;

public static class AssignAuthorEndpoint
{
    public static void Map(this IEndpointRouteBuilder app)
    {
        app.MapPut("/articles/{articleId:int}/authors/{authorId:int}", async (int articleId, int authorId,
            AssignAuthorCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(
                command with { ArticleId = articleId, AuthorId = authorId },
                cancellationToken);
            return Results.Ok(response);
        })
        .RequireRoleAuthorization(Roles.CORAUT)
        .WithName("AssignAuthor")
        .WithTags("Articles")
        .WithSummary("Assigns an author to a submission.")
        .WithDescription("This endpoint assigns an author to a specific submission based on the provided submission ID and author ID.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden);
    }
}
