using System.IO;

namespace GL.HttpServer
{
    public static class ServerEnviroment
    {
        private static string _connectionString;
        public static string Host { get; set; }

        public static string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                if (!Directory.Exists(value))
                    Directory.CreateDirectory(value);
                _connectionString = value;
            }
        }
    }
}
