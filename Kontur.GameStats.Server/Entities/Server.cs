using GL.HttpServer.Attributes;
using GL.HttpServer.Entities;
using Kontur.GameStats.Server.Dto;
using LiteDB;

namespace Kontur.GameStats.Server.Entities
{
    [MapsTo(typeof(ServerDto))]
    public class Server : Entity
    {
        public Server()
        {
            Info = new ServerInfo();
        }

        [BsonIndex(true)]
        public string Endpoint { get; set; }

        public ServerInfo Info { get; set; }
    }
}
