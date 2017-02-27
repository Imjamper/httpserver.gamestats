using System;
using System.IO;
using System.Net.Http;
using System.Text;
using GL.HttpServer;
using GL.HttpServer.Enums;
using GL.HttpServer.Mapping;
using GL.HttpServer.Types;
using HttpClient;
using Kontur.GameStats.Server.Dto;
using Kontur.GameStats.Server.UnitTests.TestModels;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal.Commands;

namespace Kontur.GameStats.Server.UnitTests.HttpServices
{
    public class ServiceTest
    {
        public const string UtcFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff'Z'";
        private HttpServer _httpServer;
        private string _port;
        private ServerDto _serverInfo;

        public ServerDto GameServer => _serverInfo;

        [SetUp]
        public void StartHttpServer()
        {
            if (_serverInfo == null)
            {
                _serverInfo = RandomGenerator.GetServer();
            }
            if (_httpServer != null)
            {
                _httpServer.Dispose();
                _httpServer = null;
            }
            if (_httpServer == null)
            {
                _httpServer = new HttpServer();
                _port = RandomGenerator.GetPort();
                ServerEnviroment.Host = $"http://+:{_port}/";
                ServerEnviroment.EnableLoggingInConsole = false;
                ServerEnviroment.InMemoryDatabase = true;
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                };
                AutoProfileLoader.Start();

                ComponentContainer.Current.Initialize();

                _httpServer.Start($"http://+:{_port}/");
            }
        }

        public ClientResponse ExecuteUrl(string path, object json, MethodType methodType)
        {
            var url = $"http://localhost:{_port}/{path}";
            using (var client = new System.Net.Http.HttpClient())
            {
                HttpContent httpContent = null;
                if (json != null)
                    httpContent = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");
                var response = new ClientResponse();
                try
                {
                    switch (methodType)
                    {
                        case MethodType.PUT:
                            using (
                                var r =
                                    client.PutAsync(new Uri(url, UriKind.Absolute), httpContent)
                                        .GetAwaiter()
                                        .GetResult())
                            {
                                string result = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                response.JsonString = result;
                                response.StatusCode = r.StatusCode.ToString();
                                break;
                            }
                        case MethodType.GET:
                            using (var r = client.GetAsync(new Uri(url, UriKind.Absolute)).GetAwaiter().GetResult())
                            {
                                string result = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                response.JsonString = result;
                                response.StatusCode = r.StatusCode.ToString();
                                break;
                            }
                        case MethodType.POST:
                            using (
                                var r =
                                    client.PostAsync(new Uri(url, UriKind.Absolute), httpContent)
                                        .GetAwaiter()
                                        .GetResult())
                            {
                                string result = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                response.JsonString = result;
                                response.StatusCode = r.StatusCode.ToString();
                                break;
                            }
                        case MethodType.DELETE:
                            using (var r = client.DeleteAsync(new Uri(url, UriKind.Absolute)).GetAwaiter().GetResult())
                            {
                                string result = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                response.JsonString = result;
                                response.StatusCode = r.StatusCode.ToString();
                                break;
                            }
                        default:
                            response.StatusCode = "888";
                            break;
                    }
                }
                catch (HttpRequestException)
                {
                    response.ErrorMessage = "Server is shout down";
                    response.StatusCode = "888";
                    return response;
                }
                catch (Exception ex)
                {
                    response.ErrorMessage = ex.Message;
                    response.StatusCode = "888";
                    return response;
                }
                return response;
            }
        }
    }
}
