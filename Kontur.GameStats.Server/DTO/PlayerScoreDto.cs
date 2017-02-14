using GL.HttpServer.Context;

namespace Kontur.GameStats.Server.DTO
{
    public class PlayerScoreDto : JsonResponse
    {
        public string Name { get; set; }

        public int Frags { get; set; }

        public int Kills { get; set; }

        public int Deaths { get; set; }
    }
}