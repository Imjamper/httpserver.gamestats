using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

namespace GL.HttpServer.Logging
{
    public class Logger
    {
        private static Serilog.Core.Logger _logger;
        public static void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            var builder = new LoggerConfiguration();
            builder = builder
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                    .WriteTo.RollingFile(
                        $"{ServerEnviroment.LoggerFolder}\\Information\\info-{DateTime.Now:yy-MM-dd}.txt"))
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                    .WriteTo.RollingFile($"{ServerEnviroment.LoggerFolder}\\Error\\error-{DateTime.Now:yy-MM-dd}.txt"));
            if (ServerEnviroment.EnableLoggingInConsole)
            {
               builder = builder.WriteTo.LiterateConsole();
            }
            _logger = builder.CreateLogger();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            Log.Error(exception, "Unhandled exception");
        }

        public static void Info(string message)
        {
            if (_logger == null)
                Initialize();
             else _logger.Information(message);
        }

        public static void Error(Exception exception, string message)
        {
            if (_logger == null)
                Initialize();
            else _logger.Error(exception, message);
        }
    }
}
