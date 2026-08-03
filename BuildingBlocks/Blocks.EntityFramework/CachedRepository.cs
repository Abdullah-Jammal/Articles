using Blocks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Blocks.EntityFramework;

public class CachedRepository<TDbContext, TEntity, TId>(TDbContext dbContext, IMemoryCache cache)
    where TDbContext : DbContext
    where TEntity : class, IEntity<TId>
    where TId : struct
{
    public IEnumerable<TEntity> GetAll()
        => cache.GetOrCreate(typeof(TEntity), entry =>
            dbContext.Set<TEntity>().AsNoTracking().ToList())!;

    public TEntity GetById(TId id)
        => GetAll().Single(e => e.Id.Equals(id));
}
