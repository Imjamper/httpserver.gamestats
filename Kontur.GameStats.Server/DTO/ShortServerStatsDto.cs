using GL.HttpServer.Context;

namespace Kontur.GameStats.Server.DTO
{
    public class ShortServerStatsDto : JsonResponse
    {
        public double AverageMatchesPerDay { get; set; }
        public string Endpoint { get; set; }
        public string Name { get; set; }
    }
}
