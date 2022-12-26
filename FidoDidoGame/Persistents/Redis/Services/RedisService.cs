using StackExchange.Redis;
using System.Text.Json;

namespace FidoDidoGame.Persistents.Redis.Services
{
    public interface IRedisService
    {
        T Get<T>(string key);
        List<T> GetAll<T>(string pattern);

        public bool Set<T>(string key, T obj, params DateTime[] expirationTime);

        bool Delete(string key);

        SortedSetEntry[] ZSGet(string key, int start, int end, Order order);

        double ZSIncre<T>(string key, double score, T member);

        bool ZSSet<T>(string key, double score, T member);

        bool ZSDelete<T>(string key, T member);
    }
    public class RedisService : IRedisService
    {
        private IDatabase db;
        private IServer server;
        public RedisService(IConnectionMultiplexer redis)
        {
            db = redis.GetDatabase();
            server = redis.GetServer("127.0.0.1", 6379);
        }

        public bool Delete(string key)
        {
            bool check = db.KeyExists(key);
            if (check)
                return db.KeyDelete(key);
            return false;
        }

        public T Get<T>(string key)
        {
            RedisValue value = db.StringGet(key)!;
            if (!string.IsNullOrEmpty(value))
                return JsonSerializer.Deserialize<T>(value!)!;
            return default!;
        }

        public List<T> GetAll<T>(string pattern)
        {
            RedisKey[] keys = server.Keys(pattern: $"*{pattern}*").ToArray();
            RedisValue[] values = db.StringGet(keys);
            return values.Select(x => JsonSerializer.Deserialize<T>(x!)).ToList()!;
        }

        public bool Set<T>(string key, T obj, params DateTime[] expirationTime)
        {
            TimeSpan? exp = null;
            if (expirationTime.Length != 0)
                 exp = expirationTime[1].Subtract(DateTime.Now);
                
            return db.StringSet(key, JsonSerializer.Serialize(obj), exp);
        }

        public bool ZSDelete<T>(string key, T member)
        {
            bool check = db.KeyExists(key);
            if (check)
            {
                db.SortedSetRemove(key, JsonSerializer.Serialize(member));
                return true;
            }
            return false;
        }

        public SortedSetEntry[] ZSGet(string key, int start, int end, Order order)
        {
            bool check = db.KeyExists(key);
            if (check)
                return db.SortedSetRangeByRankWithScores(key, start, end, order);

            return default!;
        }

        public double ZSIncre<T>(string key, double score, T member)
        {
            return db.SortedSetIncrement(key, JsonSerializer.Serialize(member), score);
        }

        public bool ZSSet<T>(string key, double score, T member)
        {
            return db.SortedSetAdd(key, JsonSerializer.Serialize(member), score);
        }
    }
}
