using GL.HttpServer.Context;
using LiteDB;

namespace Kontur.GameStats.Server.DTO
{
    public class FullServerInfo : JsonResponse
    {
        public FullServerInfo()
        {
            Info = new ServerInfo();
        }

        [BsonIndex(true)]
        public string Endpoint { get; set; }
        
        public ServerInfo Info { get; set; }
    }
}
