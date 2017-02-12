using System;
using GL.HttpServer.HttpServices;

namespace GL.HttpServer
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
            ServerEnviroment.Host = _config.Prefix;
            ServerEnviroment.ConnectionString = $"{AppDomain.CurrentDomain.BaseDirectory}/Database/LiteDB.db";
            ComponentContainer.Current.Initialize();
            using (var server = new HttpServer(_config.Prefix))
            {
                Console.WriteLine("The server is running. For turn off the server, press any key...");
                Console.ReadLine();
            }
        }
    }
}