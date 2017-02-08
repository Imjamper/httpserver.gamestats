using Kontur.GameStats.Server.Attributes;
using Kontur.GameStats.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.HttpServices
{
    [HttpService("players")]
    public class PlayersService : IHttpService
    {
        public PlayersService()
        {

        }

        /// <summary>
        /// Получить статистику игрока
        /// </summary>
        [GetOperation("stats", "/players/<name>/stats")]
        public PlayerStats GetPlayerStats(string name)
        {
            var model = new PlayerStats();
            model.AverageMatchesPerDay = 0;
            model.FavoriteGameMode = "DM";
            return model; 
        }
    }
}
