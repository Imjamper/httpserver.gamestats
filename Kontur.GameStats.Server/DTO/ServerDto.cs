using GL.HttpServer.Context;
using Kontur.GameStats.Server.DTO;
using Kontur.GameStats.Server.Entities;
using LiteDB;

namespace Kontur.GameStats.Server.Dto
{
    public class ServerDto : JsonResponse
    {
        public ServerDto()
        {
            Info = new ServerInfoDto();
        }

        public string Endpoint { get; set; }
        
        public ServerInfoDto Info { get; set; }
    }
}
