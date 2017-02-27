using System;
using GL.HttpServer.Context;
using GL.HttpServer.Types;

namespace Kontur.GameStats.Server.Dto
{
    public class MatchDto : JsonResponse
    {
        public string Server { get; set; }
        public DateOffset TimeStamp { get; set; }
        public MatchResultDto Results { get; set; }
    }
}
