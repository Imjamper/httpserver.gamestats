using System;
using System.Collections.Generic;
using System.Linq;
using GL.HttpServer.Attributes;
using GL.HttpServer.Context;
using GL.HttpServer.Database;
using GL.HttpServer.Extensions;
using GL.HttpServer.HttpServices;
using GL.HttpServer.Managers;
using GL.HttpServer.Types;
using Kontur.GameStats.Server;
using Kontur.GameStats.Server.Dto;
using Kontur.GameStats.Server.DTO;
using Kontur.GameStats.Server.Entities;
using Match = System.Text.RegularExpressions.Match;

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
                var existServer = unit.Repository<Entities.Server>().Find(a => a.Endpoint == endpoint.ToString()).FirstOrDefault();
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
        public EmptyResponse PutMatchInfo(Endpoint endpoint, DateTime timestamp, MatchResultDto body)
        {
            using (var unit = new UnitOfWork())
            {
                try
                {
                    var match = new Entities.Match();
                    match.Server = endpoint.ToString();
                    match.TimeStamp = timestamp;
                    match.Results = body.ToEntity<MatchResult>();
                    unit.Repository<Entities.Match>().Add(match);
                    return new EmptyResponse(200);
                }
                catch
                {
                    return new EmptyResponse(500);
                }
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
                try
                {
                    var existServer = unit.Repository<Entities.Server>().FindOne(a => a.Endpoint == endpoint.ToString());
                    if (existServer?.Info != null)
                    {
                        return existServer.Info.ToDto<ServerInfoDto>();
                    }
                    response.StatusCode = 404;
                    return response;
                }
                catch
                {
                    response.StatusCode = 500;
                    return response;
                }
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
                return allServers.ToJsonList<ServerDto>();
            }
        }

        /// <summary>
        ///     Получение информации о завершенном матче
        /// </summary>
        [GetOperation("matches", "/<endpoint>/matches/<timestamp>")]
        public MatchResultDto GetMatchInfo(Endpoint endpoint, DateTime timestamp)
        {
            using (var unit = new UnitOfWork())
            {
                var response = new MatchResultDto();
                try
                {
                    var match = unit.Repository<Entities.Match>()
                        .FindOne(a => a.Server == endpoint.ToString() && a.TimeStamp == timestamp);
                    if (match?.Results != null)
                    {
                        return match.Results.ToDto<MatchResultDto>();
                    }
                    response.StatusCode = 404;
                    return response;
                }
                catch
                {
                    response.StatusCode = 500;
                    return response;
                }
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
                try
                {
                    var matches = unit.Repository<Entities.Match>().Find(a => a.Server == endpoint.ToString());
                    if (matches.Count == 0)
                        return response;
                    response.TotalMatchesPlayed = matches.Count;

                    var byDay = matches.GroupBy(match => match.TimeStamp.DayOfYear).ToList();
                    response.MaximumMatchesPerDay = byDay.Max(a => a.ToList().Count);
                    response.AveragePopulation = byDay.Average(a => a.ToList().Count);

                    var playerScores = matches.ToDictionary(match => match.Server, match => match.Results.ScoreBoard);
                    response.MaximumPopulation = playerScores.Max(a => a.Value.Count);
                    response.AveragePopulation = playerScores.Average(a => a.Value.Count);

                    var maps = matches.Select(match => match.Results.Map).ToList();
                    var dictMaps = new Dictionary<string, int>();
                    foreach (var map in maps)
                    {
                        if (dictMaps.ContainsKey(map))
                            dictMaps[map]++;
                        else dictMaps.Add(map, 1);
                    }
                    var orderMaps = dictMaps.OrderBy(a => a.Value).Select(a => a.Key);
                    response.Top5Maps.AddRange(orderMaps);

                    var gameModes = matches.Select(match => match.Results.GameMode).ToList();
                    var dictModes = new Dictionary<string, int>();
                    foreach (var mode in gameModes)
                    {
                        if (dictModes.ContainsKey(mode))
                            dictModes[mode]++;
                        else dictModes.Add(mode, 1);
                    }
                    var orderModes = dictMaps.OrderBy(a => a.Value).Select(a => a.Key);
                    response.Top5GameModes.AddRange(orderModes);
                    response.StatusCode = 404;
                    return response;
                }
                catch
                {
                    response.StatusCode = 500;
                    return response;
                }
            }
        }
    }
}