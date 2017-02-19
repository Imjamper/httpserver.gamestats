using GL.HttpServer.Cache;
using GL.HttpServer.Database;
using Kontur.GameStats.Server.Dto;
using Kontur.GameStats.Server.Entities;

namespace Kontur.GameStats.Server.CacheLoaders
{
    public class ServerStatsCacheLoader : ICacheLoader
    {
        public void Execute()
        {
            using (var unit = new UnitOfWork(true))
            {
                var servers = unit.Repository<Entities.Server>().FindAll();
                if (servers == null)
                    return;
                foreach (var server in servers)
                {
                    var serverStats = new ServerStatsTempInfo();
                    var matches = unit.Repository<Match>().Find(a => a.Server == server.Endpoint.ToString());
                    serverStats.TotalMatchesPlayed = matches.Count;
                    foreach (var match in matches)
                    {
                        if (serverStats.MatchesPerDay.ContainsKey(match.TimeStamp.Value.ToString("yy-MM-dd")))
                        {
                            serverStats.MatchesPerDay[match.TimeStamp.Value.ToString("yy-MM-dd")]++;
                        }
                        else serverStats.MatchesPerDay.Add(match.TimeStamp.Value.ToString("yy-MM-dd"), 1);
                        if (serverStats.GameModes.ContainsKey(match.Results.GameMode))
                        {
                            serverStats.GameModes[match.Results.GameMode]++;
                        }
                        else serverStats.GameModes.Add(match.Results.GameMode, 1);
                        if (serverStats.Maps.ContainsKey(match.Results.Map))
                        {
                            serverStats.Maps[match.Results.Map]++;
                        }
                        else serverStats.Maps.Add(match.Results.Map, 1);
                        serverStats.Population += match.Results.ScoreBoard.Count;
                        if (serverStats.MaximumPopulation < match.Results.ScoreBoard.Count)
                            serverStats.MaximumPopulation = match.Results.ScoreBoard.Count;
                    }
                    MemoryCache.Global.AddOrUpdate(server.Endpoint, serverStats);
                }
            }
        }
    }
}
