# CacheStore

This is a unambitious thread-safe cache store for arbitrary types of objects with the assumption that keys will always be strings.

## Underlying Synchronisation Class ##

There was a dilemma in choosing the underlying locking mechanism between Monitor vs ReaderWriterLockSlim synchronisation classes, however even in the event that there are more reading threads vs writing threads the performance of Monitor was better.  ConcurrentDictionary was also considered to allow multiple read and write operations concurrently, however was we also needed to report the least recently used item when the store hit capacity this required writing to the underlying store again for that item to be removed.  ConcurrentDictionary is vastly more efficient in thread-safe implementations when there is no additional thread-safe logic required outside of the interface of ConcurrentDictionary.

## Efficiency ##

The efficiency of the implementation is highest when the number of items in the underlying store is less than the maximum capacity.  This is because under the 'least recently used' strategy we need to find the oldest access item to be removed which requires sorting and writing to the underlying cache store.  When the number of items in the underlying store is less than the maximum capacity then setting and getting of cache items is close to O(1) as the underlying cache store uses a Dictionary.  SortedDictionary was also considered to allow for the keys to be sorted based upon 'least recently used' strategy, but when the number of items in the store is less than the maximum capacity the SortedDictionary is slower than Dictionary.t

## Singleton ##

A dependency injection framework can instantiate the CacheStoreRepository as singleton rather than implementing the design pattern directly on the implementation.
