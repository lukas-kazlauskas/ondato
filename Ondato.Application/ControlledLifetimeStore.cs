using Ondato.Application.Abstractions;
using Ondato.Application.KeyValueStores;

namespace Ondato.Application
{
    public class ControlledLifetimeStore<TKey, TValue> 
        where TValue: ILifetimePolicySubject<TValue>
    {
        private readonly IKeyValueStore<TKey, TValue> _keyValueStore;
        private readonly ILifetimePolicy _lifetimePolicy;

        public ControlledLifetimeStore(
            KeyValueStoreFactory keyValueStoreFactory,
            ILifetimePolicy lifetimePolicy)
        {
            _keyValueStore = keyValueStoreFactory.Create<TKey, TValue>();
            _lifetimePolicy = lifetimePolicy;
        }

        public void Create(TKey key, TValue value)
        {
            var updated = value.Apply(_lifetimePolicy, LifetimeEvent.Created);
            _keyValueStore.Add(key, updated);
        }
        
        public TValue Get(TKey key)
        {
            var result = _keyValueStore.Get(key);

            if (result == null)
            {
                return default;
            }
            
            if (result.HasLifetimeEnded)
            {
                _keyValueStore.Remove(key);
                return default;
            }

            var updated = result.Apply(_lifetimePolicy, LifetimeEvent.Read);
            _keyValueStore.Update(key, updated);
            return updated;
        }

        public TValue Update(TKey key, TValue value)
        {
            var existing = _keyValueStore.Get(key);

            if (existing == null)
            {
                return default;
            }

            var updated = value.Apply(_lifetimePolicy, LifetimeEvent.Update);
            _keyValueStore.Update(key, updated);
            return updated;
        }
        
        public void Remove(TKey key)
        {
            _keyValueStore.Remove(key);
        }
    }
}