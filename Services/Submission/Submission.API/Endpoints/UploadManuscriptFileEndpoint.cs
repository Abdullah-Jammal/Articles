using Article.Security;
using Articles.Abstractions;
using Articles.Abstractions.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Submission.Application.Features.UploadFile.UploadManuscriptFile;

namespace Submission.API.Endpoints;

public static class UploadManuscriptFileEndpoint
{
    public static IEndpointRouteBuilder Map(this IEndpointRouteBuilder app)
    {
        app.MapPost("/articles/{articleId:int}/assets/manuscript:upload",
            async ([FromRoute] int articleId, [FromForm] UploadManuscriptFileCommand command, ISender sender) =>
            {
                var response = await sender.Send(command with { ArticleId = articleId });
                return Results.Created($"/articles/{articleId}/assets/manuscript", response);
            })
        .RequireRoleAuthorization(Role.CORAUT)
        .WithName("UploadManuscriptFile")
        .WithTags("Assets")
        .Produces<IdResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .ProducesProblem(StatusCodes.Status503ServiceUnavailable)
        .DisableAntiforgery();

        return app;
    }
}
