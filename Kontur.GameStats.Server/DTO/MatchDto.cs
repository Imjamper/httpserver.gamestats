using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Context;
using GL.HttpServer.Entities;
using Kontur.GameStats.Server.Entities;

namespace Kontur.GameStats.Server.Dto
{
    public class MatchDto : JsonResponse
    {
        public string Server { get; set; }
        public DateTime TimeStamp { get; set; }
        public MatchResultDto Results { get; set; }
    }
}
