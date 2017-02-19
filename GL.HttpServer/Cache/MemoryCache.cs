using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GL.HttpServer.Cache
{
    public class MemoryCache : IDisposable
    {

        public MemoryCache()
        {
            
        }

        private static Lazy<MemoryCache> _global = new Lazy<MemoryCache>();
        private Dictionary<string, object> _cache = new Dictionary<string, object>();
        private ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();
        private bool _disposed = false;

        public static MemoryCache Global => _global.Value;

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

        public void AddOrUpdate(string key, object cacheObject)
        {
            Task.Factory.StartNew(() =>
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

        public object this[string key] => Get<object>(key);

        public T Get<T>(string key) where T: class 
        {
            if (_disposed)
                return default(T);

            _locker.EnterReadLock();
            try
            {
                object rv;
                if (_cache.TryGetValue(key, out rv))
                {
                    return rv as T;
                }
                return null;
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        public bool TryGetValue<T>(string key, out T value) where T:class 
        {
            if (_disposed)
            {
                value = default(T);
                return false;
            }

            _locker.EnterReadLock();
            try
            {
                object rv;
                if (_cache.TryGetValue(key, out rv))
                {
                    value = rv as T;
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
}
