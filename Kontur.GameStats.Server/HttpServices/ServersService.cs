using Kontur.GameStats.Server.Attributes;
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
        public void Info()
        {

        }
    }
}
