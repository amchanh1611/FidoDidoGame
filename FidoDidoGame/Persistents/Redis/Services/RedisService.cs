using FidoDidoGame.Common.DateTimeConverter;
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

        List<T> ZSGetByScores<T>(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, Order order = Order.Ascending, int skip = 0, int take = -1);
        List<T> ZSGet<T>(string key, int start = 0, int end = -1, Order order = Order.Descending);
        long? ZsGetRank<T>(string key, T member, Order order = Order.Descending);

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
            return Array.ConvertAll(values, x=> JsonSerializer.Deserialize<T>(x!)).ToList()!;
        }

        public bool Set<T>(string key, T obj, params DateTime[] expirationTime)
        {
            TimeSpan? exp = expirationTime.Length != 0 ? expirationTime[0].Subtract(DateTime.Now) : null;

            return db.StringSet(key, JsonSerializer.Serialize(obj), exp);
        }

        public bool ZSDelete<T>(string key, T member)
        {
            bool check = db.KeyExists(key);
            if (check)
            {
                //    JsonSerializerOptions AuthAppBuilderExtensions = new JsonSerializerOptions();
                //    AuthAppBuilderExtensions.Converters.Add(new DateTimeConverter());
                //    var a = JsonSerializer.Serialize(member, AuthAppBuilderExtensions);

                db.SortedSetRemove(key, JsonSerializer.Serialize(member));
                return true;
            }
            return false;
        }

        public List<T> ZSGet<T>(string key, int start = 0, int end = -1, Order order = Order.Descending)
        {
            bool check = db.KeyExists(key);
            if (check)
            {
                RedisValue[] values = db.SortedSetRangeByRank(key, start, end, order);
                return Array.ConvertAll(values, x => JsonSerializer.Deserialize<T>(x!)).ToList()!;
            }
            return default!;
        }

        public SortedSetEntry[] ZSGetWithScores(string key, int start = 0, int end = -1, Order order = Order.Descending)
        {
            bool check = db.KeyExists(key);
            if (check)
                return db.SortedSetRangeByRankWithScores(key, start, end, order);

            return default!;
        }
        public List<T> ZSGetByScores<T>(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, Order order = Order.Ascending, int skip = 0, int take = -1)
        {
            bool check = db.KeyExists(key);
            if(check)
            {
                RedisValue[] values = db.SortedSetRangeByScore(key, min, max, order: order, skip: skip, take: take);
                return Array.ConvertAll(values, x => JsonSerializer.Deserialize<T>(x!)).ToList()!;
            }
            return default!;
        }

        public double ZSIncre<T>(string key, double score, T member)
        {
            //    JsonSerializerOptions AuthAppBuilderExtensions = new JsonSerializerOptions();
            //    AuthAppBuilderExtensions.Converters.Add(new DateTimeConverter());
            //    var a = JsonSerializer.Serialize(member, AuthAppBuilderExtensions);
            return db.SortedSetIncrement(key, JsonSerializer.Serialize(member), score);
        }

        public bool ZSSet<T>(string key, double score, T member)
        {
            return db.SortedSetAdd(key, JsonSerializer.Serialize(member), score);
        }

        public long? ZsGetRank<T>(string key, T member, Order order = Order.Descending)
        {
            return db.SortedSetRank(key, JsonSerializer.Serialize(member), order);
        }
    }
}
