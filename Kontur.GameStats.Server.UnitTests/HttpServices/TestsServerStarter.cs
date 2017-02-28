using System;
using GL.HttpServer;
using GL.HttpServer.Types;
using Kontur.GameStats.Server.HttpServices;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Kontur.GameStats.Server.UnitTests.HttpServices
{
    [SetUpFixture]
    public class TestsServerStarter
    {
        public const string UtcFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ff'Z'";
        private static string _port;
        private HttpServer _httpServer;
        private readonly Random _random = new Random();

        public static string Port => _port;

        [OneTimeSetUp]
        public void StartHttpServer()
        {
            if (_httpServer == null)
            {
                _httpServer = new HttpServer();
                _port = GetPort();
                ServerEnviroment.Host = $"http://+:{_port}/";
                ServerEnviroment.EnableLoggingInConsole = false;
                ServerEnviroment.InMemoryDatabase = true;
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    DateFormatString = UtcFormat,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat
                };

                ComponentContainer.Current.Initialize(typeof(ServersService).Assembly);

                _httpServer.Start($"http://+:{_port}/");
            }
        }

        public string GetPort()
        {
            return _random.Next(65535).ToString();
        }
    }
}
