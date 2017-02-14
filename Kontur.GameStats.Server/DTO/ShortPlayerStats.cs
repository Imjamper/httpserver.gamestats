using GL.HttpServer.Context;

namespace Kontur.GameStats.Server.DTO
{
    public class ShortPlayerStats : JsonResponse
    {
        public string Name { get; set; }
        public double KillToDeathRatio { get; set; }
    }
}
