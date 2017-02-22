using GL.HttpServer.Cache;
using GL.HttpServer.Database;
using Kontur.GameStats.Server.DTO.CacheInfo;
using Kontur.GameStats.Server.Entities;

namespace Kontur.GameStats.Server.CacheLoaders
{
    public class PlayerStatsCacheLoader : ICacheLoader
    {
        public void Load()
        {
            using (var unit = new UnitOfWork(true))
            {
                var servers = unit.Repository<Entities.Server>().FindAll();
                if (servers == null)
                    return;
                foreach (var server in servers)
                {
                    var matches = unit.Repository<Match>().Find(a => a.Server == server.Endpoint.ToString());
                    foreach (var match in matches)
                    {
                        var players = match.Results.ScoreBoard;
                        foreach (var player in players)
                        {
                            PlayerStatsTempInfo playerStats;
                            if (MemoryCache.Cache<PlayerStatsTempInfo>().TryGetValue(player.Name, out playerStats))
                            {
                                playerStats.Update(match);
                            }
                            else
                            {
                                playerStats = new PlayerStatsTempInfo(player.Name, match);
                            }
                            MemoryCache.Cache<PlayerStatsTempInfo>().PutAsync(player.Name, playerStats);
                        }
                    }
                }
            }
        }
    }
}
