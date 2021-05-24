using System;

namespace CacheStore
{
    public class CacheStoreRemovedItemEventArgs : EventArgs
    {
        public CacheStoreItem Item { get; set; }

        public CacheStoreRemovedItemEventArgs(CacheStoreItem item)
        {
            this.Item = item;
        }
    }

}
