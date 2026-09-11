using Redis.OM;
using Redis.OM.Searching;
using StackExchange.Redis;
using Journals.Domain.Journals;

namespace Journals.Persistence;

public class JournalDbContext
{
    private readonly RedisConnectionProvider _provider;
    private readonly IDatabase _redisDB;
    public JournalDbContext(IConnectionMultiplexer redis, RedisConnectionProvider provider)
        => (_redisDB, _provider) = (redis.GetDatabase(), provider);

    public IRedisCollection<Journal> Journal => _provider.RedisCollection<Journal>();
    public IRedisCollection<Editor> Editor => _provider.RedisCollection<Editor>();

    public RedisConnectionProvider Provider => _provider;
}
