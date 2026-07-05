using Blocks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blocks.EntityFramework;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    Task<TEntity?> FindByIdAsync(int id);
    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity> AddAsync(TEntity entity);
    TEntity Update(TEntity entity);
    void Remove(TEntity entity);
    Task<bool> DeleteByIdAsync(int id);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public class Repository<TContext, TEntity>
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

    public async Task<TEntity?> FindByIdAsync(int id)
    {
        return await _entity.FindAsync(id);
    }

    public TContext Context => _dbcontext;
    public virtual DbSet<TEntity> Entities => _entity;
    protected virtual IQueryable<TEntity> Query() => _entity;

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _entity.FindAsync(id);
    }
    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await _entity.AddAsync(entity);
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
    public virtual async Task<bool> DeleteByIdAsync(int id)
    {
        var rowsAffected = await _dbcontext.Database
            .ExecuteSqlInterpolatedAsync($"DELETE FROM {_entity.EntityType.GetTableName()} WHERE Id = {id}");
        return rowsAffected > 0;
    }
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _dbcontext.SaveChangesAsync(ct);
    }
}
