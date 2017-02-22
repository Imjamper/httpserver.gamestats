namespace GL.HttpServer.Cache
{
    public interface IMemoryCache
    {
        void Clear();
        object this[string key] { get; }
        void Remove(string key);
        bool Contains(string key);
    }
}