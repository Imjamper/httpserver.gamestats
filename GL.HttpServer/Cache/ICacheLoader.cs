namespace GL.HttpServer.Cache
{
    /// <summary>
    /// Точка расширения для загрузки данных в кеш, при запуске сервера
    /// </summary>
    public interface ICacheLoader
    {
        /// <summary>
        /// Реализация метода загрузки
        /// </summary>
        void Load();
    }
}
