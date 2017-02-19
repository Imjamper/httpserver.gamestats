namespace GL.HttpServer
{
    public class Configuration
    {
        /// <summary>
        ///     Префикс для прослушивания
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        ///     Включить отображение логов в консоль
        /// </summary>
        public bool EnableLogging { get; set; }
    }
}