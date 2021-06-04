# Description
This is ASP .NET Core WEB API application source code for storing and managing keys and corresponding value collections.

## Configuration
The app can be configured by changing `appsettings.json`

```javascript
"DictionaryConfig": {
    "KeyValueStoreType": "InMemory",
    "MaxExpireInSeconds": 30,
    "CleanUpInSeconds": 60
  }
```

* `KeyValueStoreType` - allows `InMemory` and `Database` to switch between these two implementations (see below)
* `MaxExpireInSeconds` - maximum allowed expiration period for values as well as default expiration period, when custom one is not provided
* `CleanUpInSeconds` - clean up period for stores, that support periodic cleanup of expired values


## Implementation details
#### Value container
The values from API are stored to immutable `ExpirableValueCollection<T>` generic which stores collection of `T` values and expiration data for those values. This, with combination of given key, are stored to the `ControlledLifetimeStore<TKey, TValue>`.

#### Managing value lifetime
The lifetime of values are determined by `ILifetimePolicy` and are applied by `ControlledLifetimeStore<TKey, TValue>`.

One policy is currently available (`DefaultLifetimePolicy`) which extends expiration based on subject's own defined lifetime duration and configured maximum allowed duration.

The `ControlledLifetimeStore<TKey, TValue>` applies given `ILifetimePolicy` in order to modify expiration for values. It also reactively remove any expired values when those are fetched. 

The key-value pairs are stored to a provided `IKeyValueStore<TKey, TValue>`.

#### Storing values

The key values are stored in underlying `IKeyValueStore<TKey, TValue>` that has two implementations in this app:
* `InMemoryKeyValueStore` (non-persistent) that stores values based on keys in a thread-safe, concurrent dictionary. This implementation supports proactive cleanup of values, based on provided `ICleanupPolicy`.
* `DatabaseKeyValueStore` (persistent) that stores values based on keys in configured database. It uses EF Core and MS SQL Server to stores keys and values as `JSON`.
