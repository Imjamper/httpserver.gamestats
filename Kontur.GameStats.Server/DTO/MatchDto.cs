using System;
using GL.HttpServer.Context;
using LiteDB;

namespace Kontur.GameStats.Server.Dto
{
    public class MatchDto : JsonResponse
    {
        public string Server { get; set; }
        [BsonIndex]
        public DateTimeOffset TimeStamp { get; set; }
        public MatchResultDto Results { get; set; }
    }
}
