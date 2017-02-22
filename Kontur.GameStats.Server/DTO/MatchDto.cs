using System;
using GL.HttpServer.Context;

namespace Kontur.GameStats.Server.Dto
{
    public class MatchDto : JsonResponse
    {
        public string Server { get; set; }
        public DateTime? TimeStamp { get; set; }
        public MatchResultDto Results { get; set; }
    }
}
