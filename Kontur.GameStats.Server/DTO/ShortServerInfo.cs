using GL.HttpServer.Context;

namespace Kontur.GameStats.Server.DTO
{
    public class ShortServerStats : JsonResponse
    {
        public double AverageMatchesPerDay { get; set; }
        public string Endpoint { get; set; }
        public string Name { get; set; }
    }
}
