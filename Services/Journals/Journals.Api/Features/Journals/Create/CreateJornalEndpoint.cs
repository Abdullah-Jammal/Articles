using Articles.Abstractions;
using Articles.Abstractions.Enums;
using Blocks.Exceptions;
using Blocks.Redis;
using FastEndpoints;
using Journals.Domain.Journals;
using Journals.Domain.Journals.Events;
using Mapster;
using Microsoft.AspNetCore.Authorization;

namespace Journals.Api.Features.Journals.Create;

[Authorize(Roles =Role.EOF)]
[HttpPost("Journals")]
[Tags("Journals")]
public class CreateJornalEndpoint
    (Repository<Journal> journalRepository, Repository<Editor> editorRepository)
    : Endpoint<CreateJornalCommand, IdResponse>
{
    public async override Task HandleAsync(CreateJornalCommand command, CancellationToken ct)
    {
        if(journalRepository.Collection.Any(j => j.Abbreviation == command.Abbreviation || j.Name == command.Name))
            throw new BadRequestException("Journal with the same name or abbreviation already exists.");

        if(!editorRepository.Collection.Any(e => e.Id == command.ChiefEditorId))
        { 
           // to do
        };

        var journal = command.Adapt<Journal>();

        await journalRepository.AddAsync(journal);
        await journalRepository.SaveAllAsync();

        await PublishAsync(new JournalCreated(journal));
        await Send.OkAsync(new IdResponse(journal.Id), cancellation: ct);
    }
}
