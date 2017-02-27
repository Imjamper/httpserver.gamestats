using System;
using System.IO;
using GL.HttpServer.Extensions;
using Serilog;
using Serilog.Events;

namespace GL.HttpServer.Logging
{
    public class Logger
    {
        private static Serilog.Core.Logger _logger;
        private static void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            var builder = new LoggerConfiguration();
            if (!ServerEnviroment.LoggerFolder.IsNullOrEmpty())
            {
                builder = builder
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                    .WriteTo.RollingFile(
                        Path.Combine($"{ServerEnviroment.LoggerFolder}", "Information", $"info-{DateTime.Now:yy-MM-dd}.txt")))
                .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                    .WriteTo.RollingFile(
                        Path.Combine($"{ServerEnviroment.LoggerFolder}", "Error", $"error-{DateTime.Now:yy-MM-dd}.txt")));
                if (ServerEnviroment.EnableLoggingInConsole)
                {
                    builder = builder.WriteTo.LiterateConsole();
                }
                _logger = builder.CreateLogger();
            }
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
            _logger?.Information(message);
        }

        public static void Error(Exception exception, string message)
        {
            if (_logger == null)
                Initialize();
            _logger?.Error(exception, message);
        }

        public static void Error(Exception exception)
        {
            if (_logger == null)
                Initialize();
            _logger?.Error(exception, exception.Message);
        }
    }
}
