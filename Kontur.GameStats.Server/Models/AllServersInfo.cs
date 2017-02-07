using Kontur.GameStats.Server.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Models
{
    public class AllServersInfo : JsonResponse
    {
        public AllServersInfo()
        {
            ServersInfo = new List<ServerInfo>();
        }

        public List<ServerInfo> ServersInfo { get; set; }
    }
}
