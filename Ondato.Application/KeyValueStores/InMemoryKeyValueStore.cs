using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Ondato.Application.Abstractions;

namespace Ondato.Application.KeyValueStores
{
    public class InMemoryKeyValueStore<TKey, TValue> : IKeyValueStore<TKey, TValue>, ICleanable<TValue>
    {
        private readonly ICleanupPolicy<TValue> _cleanupPolicy;
        private readonly ConcurrentDictionary<TKey, TValue> _dictionary;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public InMemoryKeyValueStore(ICleanupPolicy<TValue> cleanupPolicy)
        {
            _cleanupPolicy = cleanupPolicy;
            _dictionary = new ConcurrentDictionary<TKey, TValue>();
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() => StartPeriodicCleanup(_cancellationTokenSource.Token));
        }
        
        ~InMemoryKeyValueStore()
        {
            _cancellationTokenSource.Cancel();
        }

        public TValue Get(TKey key)
        {
            return _dictionary.TryGetValue(key, out var result) 
                ? result 
                : default;
        }

        public void Add(TKey key, TValue value)
        {
            _dictionary.TryAdd(key, value);
        }

        public void Remove(TKey key)
        {
            _dictionary.TryRemove(key, out _);
        }

        public void Update(TKey key, TValue value)
        {
            if (_dictionary.TryGetValue(key, out var existing))
            {
                _dictionary.TryUpdate(key, value, existing);
            }
        }

        void ICleanable<TValue>.Clean(Predicate<TValue> predicate)
        {
            foreach (var (key, value) in _dictionary)
            {
                if (predicate(value))
                {
                    _dictionary.TryRemove(key, out _);
                }
            }
        }

        private async Task StartPeriodicCleanup(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                _cleanupPolicy.Apply(this);
                await Task.Delay(_cleanupPolicy.NextCleanupAfter, token);
            }
        }
    }
}