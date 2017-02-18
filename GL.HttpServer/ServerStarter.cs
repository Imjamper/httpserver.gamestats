using System;
using GL.HttpServer.HttpServices;
using GL.HttpServer.Mapping;
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
            ServerEnviroment.ConnectionString = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\StatsServerDB";
            ServerEnviroment.LoggerFolder = $"{AppDomain.CurrentDomain.BaseDirectory}\\logs";
            AutoProfileLoader.Start();
            ComponentContainer.Current.Initialize();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Log.Logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole()
                .WriteTo.RollingFile($"{ServerEnviroment.LoggerFolder}\\log-{DateTime.Now.Date:yy-MM-dd}.txt")
                .CreateLogger();
                        
            using (var server = new HttpServer(_config.Prefix))
            {
                Console.WriteLine("The server is running. For turn off the server, press any key...");
                Console.ReadLine();
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            Log.Error(exception, "Unhandled exception");
        }
    }
}