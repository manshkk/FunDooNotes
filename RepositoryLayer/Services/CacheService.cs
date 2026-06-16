using Newtonsoft.Json;
using RepositoryLayer.Interfaces;
using StackExchange.Redis;

namespace RepositoryLayer.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _db;

        public CacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public T GetData<T>(string key)
        {
            var value = _db.StringGet(key);

            if (value.IsNullOrEmpty)
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(value);
        }

        public bool SetData<T>(
            string key,
            T value,
            DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime =
                expirationTime - DateTimeOffset.Now;

            return _db.StringSet(
                key,
                JsonConvert.SerializeObject(value),
                expiryTime);
        }

        public bool RemoveData(string key)
        {
            return _db.KeyDelete(key);
        }
    }
}