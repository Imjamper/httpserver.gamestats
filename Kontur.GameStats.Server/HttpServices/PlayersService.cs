using GL.HttpServer.Attributes;
using GL.HttpServer.HttpServices;
using Kontur.GameStats.Server.Models;

namespace Kontur.GameStats.Server.HttpServices
{
    [HttpService("players")]
    public class PlayersService : IHttpService
    {
        /// <summary>
        ///     Получить статистику игрока
        /// </summary>
        [GetOperation("stats", "/<name>/stats")]
        public PlayerStats GetPlayerStats(string name)
        {
            var model = new PlayerStats();
            model.AverageMatchesPerDay = 0;
            model.FavoriteGameMode = "DM";
            return model;
        }
    }
}