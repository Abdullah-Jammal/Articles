using Articles.Abstractions;
using Articles.Abstractions.Enums;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace Journals.Api.Features.Journals.Create;

[Authorize(Roles =Role.EOF)]
[HttpPost("Journals")]
[Tags("Journals")]
public class CreateJornalEndpoint : Endpoint<CreateJornalCommand, IdResponse>
{
    public async override Task HandleAsync(CreateJornalCommand command, CancellationToken ct)
    {
        await base.HandleAsync(command, ct);
    }
}
