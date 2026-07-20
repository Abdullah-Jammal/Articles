using Blocks.Domain.Entities;
using Blocks.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Blocks.EntityFramework;

public static class RepositoryExtensions
{
    public static async Task<TEntity> FindByIdOrThrowAsync<TContext, TEntity>(
        this Repository<TContext, TEntity> repository,
        int id,
        CancellationToken cancellationToken = default)
        where TContext : DbContext
        where TEntity : class, IEntity
    {
        var entity = await repository.FindByIdAsync(id, cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException(
                $"{typeof(TEntity).Name} with id {id} was not found.");
        }

        return entity;
    }
}
