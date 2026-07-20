using Blocks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blocks.EntityFramework;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<TEntity?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    TEntity Update(TEntity entity);
    void Remove(TEntity entity);
    Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public class Repository<TContext, TEntity>
    : IRepository<TEntity>
    where TContext : DbContext
    where TEntity : class, IEntity
{
    protected readonly TContext _dbcontext;
    protected readonly DbSet<TEntity> _entity;
    public Repository(TContext dbcontext)
    {
        _dbcontext = dbcontext;
        _entity = dbcontext.Set<TEntity>();
    }

    public async Task<TEntity?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _entity.FindAsync([id], cancellationToken);
    }

    public TContext Context => _dbcontext;
    public virtual DbSet<TEntity> Entities => _entity;
    protected virtual IQueryable<TEntity> Query() => _entity;

    public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Query().SingleOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }
    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _entity.AddAsync(entity, cancellationToken);
        return entity;
    }
    public virtual TEntity Update(TEntity entity)
    {
        _entity.Update(entity);
        return entity;
    }
    public virtual void Remove(TEntity entity)
    {
        _entity.Remove(entity);
    }
    public virtual async Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var rowsAffected = await _entity
            .Where(entity => entity.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        return rowsAffected > 0;
    }
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _dbcontext.SaveChangesAsync(ct);
    }
}
