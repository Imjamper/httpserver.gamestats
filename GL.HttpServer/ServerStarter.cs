using System;
using GL.HttpServer.HttpServices;
using GL.HttpServer.Mapping;
using GL.HttpServer.Types;
using LiteDB;
using Serilog;

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
            ServerEnviroment.EnableLoggingInConsole = _config.EnableLogging;
            ServerEnviroment.ConnectionString = $"{AppDomain.CurrentDomain.BaseDirectory}\\Database";
            ServerEnviroment.LoggerFolder = $"{AppDomain.CurrentDomain.BaseDirectory}\\Logs";
            AutoProfileLoader.Start();
            ComponentContainer.Current.Initialize();
                        
            using (var server = new HttpServer(_config.Prefix))
            {
                Console.WriteLine("The server is running. For turn off the server, press any key...");
                Console.ReadLine();
            }
        }


    }
}