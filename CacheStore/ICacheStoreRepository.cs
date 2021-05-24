using System;

namespace CacheStore
{
    public interface ICacheStoreRepository
    {
        int Capacity { get; }
        int Count { get; }

        event EventHandler<CacheStoreRemovedItemEventArgs> ItemRemoved;

        object Get(string key);
        bool Remove(string key);
        void Set(string key, object value);
    }
}