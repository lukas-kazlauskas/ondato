namespace Ondato.Application.Abstractions
{
    public interface IKeyValueStore<TKey, TValue>
    {
        TValue Get(TKey key);
        void Add(TKey key, TValue value);
        void Remove(TKey key);
        void Update(TKey key, TValue value);
    }
}