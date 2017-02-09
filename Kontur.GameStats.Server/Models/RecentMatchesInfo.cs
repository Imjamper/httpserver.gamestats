using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Context;

namespace Kontur.GameStats.Server.Models
{
    public class RecentMatchesInfo : JsonResponse
    {
        public RecentMatchesInfo()
        {
            Results = new List<MatchInfo>();
        }

        public string Server { get; set; }
        public DateTime TimeStamp { get; set; }
        public List<MatchInfo> Results { get; set; }
    }
}
