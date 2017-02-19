using GL.HttpServer.Attributes;
using GL.HttpServer.HttpServices;
using Kontur.GameStats.Server.Dto;
using Kontur.GameStats.Server.DTO;

namespace Kontur.GameStats.Server.HttpServices
{
    [HttpService("players")]
    public class PlayersService : IHttpService
    {
        /// <summary>
        ///     Получить статистику игрока
        /// </summary>
        [GetOperation("stats", "/<name>/stats")]
        public PlayerStatsDto GetPlayerStats(string name)
        {
            var model = new PlayerStatsDto();
            model.AverageMatchesPerDay = 0;
            model.FavoriteGameMode = "DM";
            return model;
        }
    }
}