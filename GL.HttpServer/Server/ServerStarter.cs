using System;

namespace GL.HttpServer.Server
{
    public class ServerStarter
    {
        private readonly Configuration _config;

        public ServerStarter(Configuration configuration)
        {
            _config = configuration;
        }

        public void Start()
        {
            using (var server = new StatServer(_config.Prefix))
            {
                Console.WriteLine("Server started. For terminate press any key...");
                Console.ReadLine();
            }
        }
    }
}