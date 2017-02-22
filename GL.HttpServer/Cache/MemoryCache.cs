using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GL.HttpServer.Cache
{
    public class MemoryCache<T> : IMemoryCache
    {
        public Type CacheType => typeof(T);

        private readonly Dictionary<string, T> _cache = new Dictionary<string, T>();
        private readonly ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;

                if (disposing)
                {
                    Clear();
                    _locker.Dispose();
                }
            }
        }

        public void Clear()
        {
            _locker.EnterWriteLock();
            try
            {
                _cache.Clear();
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        public Task PutAsync(string key, T cacheObject)
        {
            return Task.Factory.StartNew(() =>
            {
                if (_disposed) return;

                _locker.EnterUpgradeableReadLock();
                try
                {
                    if (!_cache.ContainsKey(key))
                        _cache.Add(key, cacheObject);
                    else
                        _cache[key] = cacheObject;
                }
                finally
                {
                    _locker.ExitUpgradeableReadLock();
                }
            });
        }

        public object this[string key] => Get(key);

        public T Get(string key) 
        {
            if (_disposed)
                return default(T);

            _locker.EnterReadLock();
            try
            {
                T rv;
                if (_cache.TryGetValue(key, out rv))
                {
                    return rv;
                }
                return default(T);
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        public List<T> GetAll()
        {
            if (_disposed)
                return new List<T>();

            _locker.EnterReadLock();
            try
            {
                return _cache.Select(k => k.Value).ToList();
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        public bool TryGetValue(string key, out T value)
        {
            if (_disposed)
            {
                value = default(T);
                return false;
            }

            _locker.EnterReadLock();
            try
            {
                T rv;
                if (_cache.TryGetValue(key, out rv))
                {
                    value = rv;
                    return true;
                }
                value = default(T);
                return false;
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        public void Remove(string key)
        {
            if (_disposed) return;

            _locker.EnterWriteLock();
            try
            {
                if (_cache.ContainsKey(key))
                {
                    _cache.Remove(key);
                }
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        public bool Contains(string key)
        {
            if (_disposed) return false;

            _locker.EnterReadLock();
            try
            {
                return _cache.ContainsKey(key);
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }
    }

    public class MemoryCache
    {
        private static readonly Lazy<Dictionary<Type, IMemoryCache>> LazyCaches = new Lazy<Dictionary<Type, IMemoryCache>>();
        private static readonly Dictionary<Type, IMemoryCache> Caches = LazyCaches.Value;

        public static MemoryCache<T> Cache<T>()
        {
            IMemoryCache cache;
            if (Caches.TryGetValue(typeof(T), out cache))
            {
                return cache as MemoryCache<T>;
            }
            cache = new MemoryCache<T>();
            Caches.Add(typeof(T), cache);
            return (MemoryCache<T>) cache;
        }
    }
}
