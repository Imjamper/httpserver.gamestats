using System;
using GL.HttpServer.HttpServices;
using GL.HttpServer.Mapping;

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
            ServerEnviroment.ConnectionString = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\StatsServerDB";
            AutoProfileLoader.RegisterDomain();
            ComponentContainer.Current.Initialize();
            using (var server = new HttpServer(_config.Prefix))
            {
                Console.WriteLine("The server is running. For turn off the server, press any key...");
                Console.ReadLine();
            }
        }
    }
}