using System.Collections.Generic;
using GL.HttpServer.Context;
using GL.HttpServer.Entities;

namespace Kontur.GameStats.Server.DTO
{
    public class ServerInfoDto : JsonResponse
    {
        public ServerInfoDto()
        {
            GameModes = new List<string>();
        }
        public string Name { get; set; }
        public List<string> GameModes { get; set; }
    }
}