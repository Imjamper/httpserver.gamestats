﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GL.HttpServer.Attributes;
using GL.HttpServer.Cache;
using GL.HttpServer.Context;
using GL.HttpServer.Database;
using GL.HttpServer.Extensions;
using GL.HttpServer.HttpServices;
using GL.HttpServer.Types;
using Kontur.GameStats.Server.Dto;
using Kontur.GameStats.Server.DTO;
using Kontur.GameStats.Server.DTO.CacheInfo;
using Kontur.GameStats.Server.Entities;
using Serilog;

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
        public EmptyResponse PutMatchInfo(Endpoint endpoint, DateTime? timestamp, MatchResultDto body)
        {
            using (var unit = new UnitOfWork())
            {
                var match = new Match();
                match.Server = endpoint.ToString();
                match.TimeStamp = timestamp;
                match.Results = body.ToEntity<MatchResult>();
                unit.Repository<Match>().Add(match);
                Task.Factory.StartNew(() =>
                {
                    var serverStats = MemoryCache.Global.Get<ServerStatsTempInfo>(endpoint.ToString());
                    if (serverStats != null)
                        serverStats.Update(match);
                    else serverStats = new ServerStatsTempInfo(match);
                    MemoryCache.Global.AddOrUpdate(endpoint.ToString(), serverStats);

                    var players = match.Results.ScoreBoard;
                    foreach (var playerScore in players)
                    {
                        var playerStats = MemoryCache.Global.Get<PlayerStatsTempInfo>(playerScore.Name);
                        if (playerStats != null)
                            serverStats.Update(match);
                        else playerStats = new PlayerStatsTempInfo(playerScore.Name, match);
                        MemoryCache.Global.AddOrUpdate(playerScore.Name, playerStats);
                    }
                });

                return new EmptyResponse(200);
            }
        }

        /// <summary>
        ///     Получить информацию о сервере
        /// </summary>
        [GetOperation("info", "/<endpoint>/info")]
        public ServerInfoDto GetServerInfo(Endpoint endpoint)
        {
            using (var unit = new UnitOfWork(true))
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
            using (var unit = new UnitOfWork(true))
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
        public MatchResultDto GetMatchInfo(Endpoint endpoint, DateTime timestamp)
        {
            using (var unit = new UnitOfWork(true))
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
            using (var unit = new UnitOfWork(true))
            {
                var response = new FullServerStatsDto();

                var serverStats = MemoryCache.Global.Get<ServerStatsTempInfo>(endpoint.ToString());
                if (serverStats == null)
                {
                    response.StatusCode = 404;
                    return response;
                }
                var server = unit.Repository<Entities.Server>().FindOne(a => a.Endpoint == endpoint.ToString());
                response.Endpoint = endpoint.ToString();
                response.Name = server.Info.Name;

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