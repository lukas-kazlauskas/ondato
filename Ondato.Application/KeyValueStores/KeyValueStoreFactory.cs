using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ondato.Application.Abstractions;
using Ondato.Core.Configuration;

namespace Ondato.Application.KeyValueStores
{
    public class KeyValueStoreFactory
    {
        private readonly OndatoConfig _config;
        private readonly IServiceProvider _serviceProvider;

        public KeyValueStoreFactory(IOptions<OndatoConfig> config, IServiceProvider serviceProvider)
        {
            _config = config.Value;
            _serviceProvider = serviceProvider;
        }

        public IKeyValueStore<TKey, TValue> Create<TKey, TValue>()
        {
            IKeyValueStore<TKey, TValue> store = _config.KeyValueStoreType switch
            {
                KeyValueStoreType.InMemory =>
                    _serviceProvider.GetRequiredService<InMemoryKeyValueStore<TKey, TValue>>(),
                KeyValueStoreType.Database =>
                    _serviceProvider.GetRequiredService<DatabaseKeyValueStore<TKey, TValue>>(),
                _ => throw new ArgumentOutOfRangeException()
            };
            return store;
        }
    }
}