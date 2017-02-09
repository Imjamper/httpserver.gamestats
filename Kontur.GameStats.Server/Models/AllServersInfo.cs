using System.Collections.Generic;
using GL.HttpServer.Context;

namespace Kontur.GameStats.Server.Models
{
    public class AllServersInfo : JsonResponse
    {
        public AllServersInfo()
        {
            ServersInfo = new List<ServerInfo>();
        }

        public List<ServerInfo> ServersInfo { get; set; }
    }
}