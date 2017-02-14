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
            if (body == null || body.Name.IsNullOrEmpty())
            {
                return new EmptyResponse(500);
            }
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
        public EmptyResponse PutMatchInfo(Endpoint endpoint, DateTime? timestamp, MatchResultDto body)
        {
            return new EmptyResponse();
        }

        /// <summary>
        ///     Получить информацию о сервере
        /// </summary>
        [GetOperation("info", "/<endpoint>/info")]
        public ServerInfoDto GetServerInfo(Endpoint endpoint)
        {
            using (var unit = new UnitOfWork())
            {
                var existServer = unit.Repository<Entities.Server>().Find(a => a.Endpoint == endpoint.ToString()).FirstOrDefault();
                if (existServer?.Info != null)
                {
                    return existServer.Info.ToDto<ServerInfoDto>();
                }
                var response = new ServerInfoDto();
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
                //return unit.Repository<Entities.Server>().FindAll().ToJsonList();
                return new JsonList<ServerDto>();
            }
        }

        /// <summary>
        ///     Получение информации о завершенном матче
        /// </summary>
        [GetOperation("matches", "/<endpoint>/matches/<timestamp>")]
        public MatchResultDto GetMatchInfo(Endpoint endpoint, DateTime? timestamp)
        {
            var model = new MatchResultDto();
            model.Map = "DM-HelloWorld";
            model.GameMode = "DM";
            model.FragLimit = 20;
            model.TimeLimit = 20;
            model.TimeElapsed = 12.345678;
            model.ScoreBoard.Add(new PlayerScoreDto() {Name = "Player1", Deaths = 2, Kills = 10, Frags = 14});
            model.ScoreBoard.Add(new PlayerScoreDto() { Name = "Player2", Deaths = 21, Kills = 4, Frags = 3});
            return model;
        }

        /// <summary>
        ///     Получение статистики о играх на сервере
        /// </summary>
        [GetOperation("stats", "/<endpoint>/stats")]
        public FullServerStats GetServerStats(Endpoint endpoint)
        {
            var model = new FullServerStats();
            model.TotalMatchesPlayed = 100500;
            model.MaximumMatchesPerDay = 33;
            model.AverageMatchesPerDay = 24.456240;
            model.MaximumPopulation = 32;
            model.AveragePopulation = 20.450000;
            model.Top5GameModes.Add("DM");
            model.Top5GameModes.Add("TDM");
            model.Top5Maps.Add("DM-HelloWorld");
            model.Top5Maps.Add("DM-1on1-Rose");
            model.Top5Maps.Add("DM-Kitchen");
            model.Top5Maps.Add("DM-Camper Paradise");
            model.Top5Maps.Add("DM-Appalachian Wonderland");
            return model;
        }
    }
}