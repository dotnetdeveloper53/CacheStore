using System;
using System.Collections.Generic;
using System.Linq;

namespace CacheStore
{

    /// <summary>
    /// A non-ambitious thread-safe cache store for arbitrary types of objects
    /// </summary>
    public class CacheStoreRepository : ICacheStoreRepository
    {
        #region Members
        private readonly int capacity;
        private readonly Dictionary<string, CacheStoreItem> store;
        private readonly object locker;
        public event EventHandler<CacheStoreRemovedItemEventArgs> ItemRemoved;
        #endregion Members

        #region Properties
        public int Capacity => this.capacity;

        public int Count
        {
            get
            {
                lock (this.locker)
                {
                    return this.store.Count;
                }
            }
        }
        #endregion Properties

        #region Constructors
        public CacheStoreRepository(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentException("Capacity of cache store must be greater than zero", nameof(capacity));

            this.capacity = capacity;
            this.locker = new object();
            this.store = new Dictionary<string, CacheStoreItem>((capacity + 1));
        }
        #endregion Constructors

        #region Methods
        public void Set(string key, object value)
        {
            CacheStoreItem removedItem = null;
            lock (this.locker)
            {
                if (this.store.Count == this.capacity)
                {
                    /*
                     * If we've reached capacity get the item
                     * from the store to be removed based upon
                     * 'least recently used' strategy
                     */
                    removedItem = this.store.Values
                        .OrderBy(item => item.LastDateTime)
                        .First();
                    this.store.Remove(removedItem.Key);
                }

                this.store[key] = new CacheStoreItem(key, value);
            }

            if (removedItem != null && this.ItemRemoved != null)
                this.ItemRemoved(this, new CacheStoreRemovedItemEventArgs(removedItem));
        }

        public object Get(string key)
        {
            lock (this.locker)
            {
                CacheStoreItem value;
                if (this.store.TryGetValue(key, out value))
                {
                    value.LastDateTime = DateTime.Now;
                    return value.Value;
                }

                return default(object);
            }
        }

        public bool Remove(string key)
        {
            lock (this.locker)
            {
                return this.store.Remove(key);
            }
        }
        #endregion Methods
    }

}
