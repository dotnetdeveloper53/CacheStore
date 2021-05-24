using System;
using System.Collections.Generic;
using System.Text;

namespace CacheStore
{
    public class CacheStoreItem
    {
        public string Key { get; private set; }
        public object Value { get; private set; }
        public DateTime LastDateTime { get; internal set; }

        public CacheStoreItem(string key, object value)
        {
            this.Key = key;
            this.Value = value;
            this.LastDateTime = DateTime.Now;
        }
    }
}
