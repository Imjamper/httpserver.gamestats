using Kontur.GameStats.Server.Attributes;
using Kontur.GameStats.Server.Context;
using Kontur.GameStats.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.HttpServices
{
    public class ServersService : IHttpService
    {
        public ServersService()
        {

        }

        /// <summary>
        /// Advertise запрос от игрового сервера
        /// </summary>
        [PutOperation("/servers/<endpoint>/info")]
        public EmptyResponse PutInfo(string endpoint, ServerInfo body)
        {
            return new EmptyResponse();
        }

        /// <summary>
        /// Получить информацию о сервере
        /// </summary>
        [GetOperation("/servers/<endpoint>/info")]
        public ServerInfo GetInfo(string endpoint)
        {
            return new ServerInfo() { GameModes = new List<string> { "DM", "Single" }, Name = "] My P3rfect Server[" };
        }

        /// <summary>
        /// Получить информацию о серверах
        /// </summary>
        [GetOperation("/servers/info")]
        public AllServersInfo GetInfo()
        {
            var model = new AllServersInfo();
            model.ServersInfo.Add(new ServerInfo() { GameModes = new List<string> { "DM", "Single" }, Name = "] My P3rfect Server[" });
            model.ServersInfo.Add(new ServerInfo() { GameModes = new List<string> { "DM1", "Single2" }, Name = "] My P3rfect Server 2[" });
            return model;
        }
    }
}
