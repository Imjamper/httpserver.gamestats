using System.Collections.Generic;
using GL.HttpServer.Attributes;
using GL.HttpServer.Entities;
using Kontur.GameStats.Server.DTO;

namespace Kontur.GameStats.Server.Entities
{
    [MapsTo(typeof(ServerInfoDto))]
    public class ServerInfo : Entity
    {
        public ServerInfo()
        {
            GameModes = new List<string>();
        }
        public string Name { get; set; }
        public List<string> GameModes { get; set; }
    }
}