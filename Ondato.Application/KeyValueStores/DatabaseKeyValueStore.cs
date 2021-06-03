using System.Linq;
using Newtonsoft.Json;
using Ondato.Application.Abstractions;
using Ondato.Data;

namespace Ondato.Application.KeyValueStores
{
    public class DatabaseKeyValueStore<TKey, TValue> : IKeyValueStore<TKey, TValue>
    {
        private readonly KeyValueStoreContext _dbContext;

        public DatabaseKeyValueStore(KeyValueStoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TValue Get(TKey key)
        {
            var keyJson = JsonConvert.SerializeObject(key);
            var value = _dbContext.KeyValuePairs.SingleOrDefault(x => x.KeyJson == keyJson);
            return value != null 
                ? JsonConvert.DeserializeObject<TValue>(value.ValueJson) 
                : default;
        }

        public void Add(TKey key, TValue value)
        {
            var entry = new KeyValuePair
            {
                KeyJson = JsonConvert.SerializeObject(key),
                ValueJson = JsonConvert.SerializeObject(value)
            };

            _dbContext.Add(entry);
            _dbContext.SaveChanges();
        }

        public void Remove(TKey key)
        {
            var keyJson = JsonConvert.SerializeObject(key);
            var value = _dbContext.KeyValuePairs.SingleOrDefault(x => x.KeyJson == keyJson);
            if (value == null)
            {
                return;
            }
            _dbContext.Remove(value);
            _dbContext.SaveChanges();
        }

        public void Update(TKey key, TValue value)
        {
            var keyJson = JsonConvert.SerializeObject(key);
            var entry = _dbContext.KeyValuePairs.SingleOrDefault(x => x.KeyJson == keyJson);
            if (entry != null)
            {
                entry.ValueJson = JsonConvert.SerializeObject(value);
                _dbContext.SaveChanges();
            }
        }
    }
}