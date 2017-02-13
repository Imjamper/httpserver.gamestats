using GL.HttpServer.Context;

namespace Kontur.GameStats.Server.Models
{
    public class ShortServerStats : JsonResponse
    {
        public double AverageMatchesPerDay { get; set; }
        public string Endpoint { get; set; }
        public string Name { get; set; }
    }
}
