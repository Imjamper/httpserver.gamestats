using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using GL.HttpServer.Logging;
using GL.HttpServer.Mapping;
using GL.HttpServer.Types;
using Newtonsoft.Json;

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
            using (var server = new HttpServer())
            {
                try
                {
                    ServerEnviroment.Host = _config.Prefix;
                    ServerEnviroment.EnableLoggingInConsole = _config.EnableLogging;
                    ServerEnviroment.ConnectionString = Path.Combine($"{AppDomain.CurrentDomain.BaseDirectory}", "Database");
                    ServerEnviroment.LoggerFolder = Path.Combine($"{AppDomain.CurrentDomain.BaseDirectory}", "Logs");
                    JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                    {
                        DateTimeZoneHandling = DateTimeZoneHandling.Utc
                    };
                    AutoProfileLoader.Start();
                    ComponentContainer.Current.Initialize();

                    server.Start(_config.Prefix);
                    Console.WriteLine("The server is running. For turn off the server, press any key...");
                    Console.ReadLine();
                }
                catch (Exception exception)
                {
                    Logger.Error(exception, "Error at server startup.");
                    Console.WriteLine("Error at server startup. Press any key...");
                    Console.ReadLine();
                }
            }
        }
    }
}