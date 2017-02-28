using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GL.HttpServer.Attributes;
using GL.HttpServer.Cache;
using GL.HttpServer.Context;
using GL.HttpServer.Database;
using GL.HttpServer.Extensions;
using GL.HttpServer.HttpServices;
using GL.HttpServer.Types;
using Kontur.GameStats.Server.CacheLoaders;
using Kontur.GameStats.Server.Dto;
using Kontur.GameStats.Server.Dto.CacheInfo;
using Kontur.GameStats.Server.DTO;
using Kontur.GameStats.Server.DTO.CacheInfo;
using Kontur.GameStats.Server.Entities;

namespace Kontur.GameStats.Server.HttpServices
{
    [HttpService("servers")]
    public class ServersService : IHttpService
    {
        /// <summary>
        ///     Advertise запрос от игрового сервера
        /// </summary>
        [PutOperation("info", "/<endpoint>/info")]
        public EmptyResponse PutServerInfo(Endpoint endpoint, ServerInfoDto body)
        {
            using (var unit = new UnitOfWork())
            {
                var existServer = unit.Repository<Entities.Server>().FindOne(a => a.Endpoint == endpoint.ToString());
                if (existServer != null)
                {
                    existServer.Info = body.ToEntity<ServerInfo>();
                    unit.Repository<Entities.Server>().Update(existServer);
                }
                else
                {
                    var fullInfo = new Entities.Server();
                    fullInfo.Endpoint = endpoint.ToString();
                    fullInfo.Info = body.ToEntity<ServerInfo>();
                    unit.Repository<Entities.Server>().Add(fullInfo);
                }
                return new EmptyResponse(200);
            }
        }

        /// <summary>
        ///     Запись информации о завершенном матче
        /// </summary>
        [PutOperation("matches", "/<endpoint>/matches/<timestamp>")]
        public EmptyResponse PutMatchInfo(Endpoint endpoint, DateTimeOffset? timestamp, MatchResultDto body)
        {
            using (var unit = new UnitOfWork())
            {
                var server = unit.Repository<Entities.Server>().FindOne(a => a.Endpoint == endpoint.ToString());
                if (server != null)
                {
                    var match = new Match();
                    match.Server = endpoint.ToString();
                    match.TimeStamp = timestamp.Value;
                    match.Results = body.ToEntity<MatchResult>();
                    unit.Repository<Match>().Add(match);
                    var serverStats = MemoryCache.Cache<ServerStatsTempInfo>().Get(endpoint.ToString());
                    if (serverStats != null)
                        serverStats.Update(match);
                    else serverStats = new ServerStatsTempInfo(match);
                    MemoryCache.Cache<ServerStatsTempInfo>().PutAsync(endpoint.ToString(), serverStats);

                    var players = match.Results.ScoreBoard;
                    foreach (var playerScore in players)
                    {
                        var playerStats = MemoryCache.Cache<PlayerStatsTempInfo>().Get(playerScore.Name);
                        if (playerStats != null)
                            playerStats.Update(match);
                        else playerStats = new PlayerStatsTempInfo(playerScore.Name, match);
                        MemoryCache.Cache<PlayerStatsTempInfo>().PutAsync(playerScore.Name, playerStats);
                    }

                    var recentMatches = MemoryCache.Cache<RecentMatchesTempInfo>().Get(RecentMatchesCacheLoader.RecentMatchesUid);
                    if (recentMatches != null && recentMatches.Count > 0)
                        recentMatches.Add(match);
                    else
                    {
                        recentMatches = new RecentMatchesTempInfo();
                        recentMatches.Add(match);
                        MemoryCache.Cache<RecentMatchesTempInfo>().PutAsync(RecentMatchesCacheLoader.RecentMatchesUid, recentMatches);
                    }
                    return new EmptyResponse(200);
                }
                return new EmptyResponse();
            }
        }

        /// <summary>
        ///     Получить информацию о сервере
        /// </summary>
        [GetOperation("info", "/<endpoint>/info")]
        public ServerInfoDto GetServerInfo(Endpoint endpoint)
        {
            using (var unit = new UnitOfWork())
            {
                var response = new ServerInfoDto();
                var existServer = unit.Repository<Entities.Server>().FindOne(a => a.Endpoint == endpoint.ToString());
                if (existServer?.Info != null)
                {
                    return existServer.Info.ToDto<ServerInfoDto>();
                }
                response.StatusCode = 404;
                return response;
            }
        }

        /// <summary>
        ///     Получить информацию о серверах
        /// </summary>
        [GetOperation("info", "/info")]
        public JsonList<ServerDto> GetAllServersInfo()
        {
            using (var unit = new UnitOfWork())
            {
                var allServers = unit.Repository<Entities.Server>().FindAll();
                var model = allServers.ToJsonList<ServerDto>();
                return model;
            }
        }

        /// <summary>
        ///     Получение информации о завершенном матче
        /// </summary>
        [GetOperation("matches", "/<endpoint>/matches/<timestamp>")]
        public MatchResultDto GetMatchInfo(Endpoint endpoint, DateTimeOffset timestamp)
        {
            using (var unit = new UnitOfWork())
            {
                var response = new MatchResultDto();
                var match = unit.Repository<Match>()
                    .FindOne(a => a.Server == endpoint.ToString() && a.TimeStamp == timestamp);
                if (match?.Results != null)
                {
                    return match.Results.ToDto<MatchResultDto>();
                }
                response.StatusCode = 404;
                return response;
            }
        }

        /// <summary>
        ///     Получение статистики о играх на сервере
        /// </summary>
        [GetOperation("stats", "/<endpoint>/stats")]
        public FullServerStatsDto GetServerStats(Endpoint endpoint)
        {
            using (var unit = new UnitOfWork())
            {
                var response = new FullServerStatsDto();

                var serverStats = MemoryCache.Cache<ServerStatsTempInfo>().Get(endpoint.ToString());
                if (serverStats == null)
                {
                    response.StatusCode = 404;
                    return response;
                }
                var server = unit.Repository<Entities.Server>().FindOne(a => a.Endpoint == endpoint.ToString());
                if (server != null)
                {
                    response.Endpoint = endpoint.ToString();
                    response.Name = server.Info.Name;
                }

                response.TotalMatchesPlayed = serverStats.TotalMatchesPlayed;

                response.MaximumMatchesPerDay = serverStats.MatchesPerDay.Max(a => a.Value);
                response.AverageMatchesPerDay = serverStats.MatchesPerDay.Average(a => a.Value);

                response.MaximumPopulation = serverStats.MaximumPopulation;
                response.AveragePopulation = (double)serverStats.Population / serverStats.TotalMatchesPlayed;

                var orderMaps = serverStats.Maps.OrderByDescending(a => a.Value).Select(a => a.Key).Take(5);
                response.Top5Maps.AddRange(orderMaps);

                var orderModes = serverStats.GameModes.OrderByDescending(a => a.Value).Select(a => a.Key).Take(5);
                response.Top5GameModes.AddRange(orderModes);
               
                return response;
            }
        }
    }
}