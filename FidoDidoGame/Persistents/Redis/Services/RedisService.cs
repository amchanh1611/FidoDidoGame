using StackExchange.Redis;
using System.Text.Json;

namespace FidoDidoGame.Persistents.Redis.Services
{
    public interface IRedisService
    {
        T Get<T>(string key);

        void Set<T>(string key, T obj, DateTime expirationTime);

        void Delete(string key);
    }
    public class RedisService : IRedisService
    {
        private IDatabase db;
        public RedisService(IConnectionMultiplexer redis)
        {
            db = redis.GetDatabase();
        }

        public void Delete(string key)
        {
            bool check = db.KeyExists(key);
            if (check)
                db.KeyDelete(key);
        }

        public T Get<T>(string key)
        {
            RedisValue value = db.StringGet(key)!;
            if (!string.IsNullOrEmpty(value))
                return JsonSerializer.Deserialize<T>(value!)!;
            return default!;
        }

        public void Set<T>(string key, T obj, DateTime expirationTime)
        {
            var exp = expirationTime.Subtract(DateTime.Now);
            db.StringSet(key, JsonSerializer.Serialize(obj), exp);
        }

    }
}
