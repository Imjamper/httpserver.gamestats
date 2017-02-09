using System.Collections.Generic;
using GL.HttpServer.Context;

namespace Kontur.GameStats.Server.Models
{
    public class ServerInfo : JsonResponse
    {
        public string Name { get; set; }

        public List<string> GameModes { get; set; }
    }
}