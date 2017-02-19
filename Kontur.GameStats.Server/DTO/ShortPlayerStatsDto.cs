using GL.HttpServer.Context;

namespace Kontur.GameStats.Server.Dto
{
    public class ShortPlayerStatsDto : JsonResponse
    {
        public ShortPlayerStatsDto()
        {
            
        }
        public string Name { get; set; }
        public double KillToDeathRatio { get; set; }
    }
}
