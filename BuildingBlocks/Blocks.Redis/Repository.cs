using Redis.OM;
using Redis.OM.Searching;
using StackExchange.Redis;

namespace Blocks.Redis;

public class Repository<T> where T : Entity
{
    private readonly IRedisCollection<T> _collection;
    private readonly IDatabase _redisDB;

    public Repository(IConnectionMultiplexer redis, RedisConnectionProvider provider)
    {
        _redisDB = redis.GetDatabase();
        _collection = provider.RedisCollection<T>();
    }

    public IRedisCollection<T> Collection => _collection;

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _collection.FindByIdAsync(id.ToString());
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        entity.Id = await GenerateNewId();
        await _collection.InsertAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        await _collection.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            await _collection.DeleteAsync(entity);
        }
    }

    public async Task SaveAllAsync() => await _collection.SaveAsync();

    public async Task<int> GenerateNewId() => (int)await _redisDB.StringIncrementAsync($"{typeof(T).Name}:Id:Sequence");
}
