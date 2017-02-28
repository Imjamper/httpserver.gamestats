using System;
using GL.HttpServer.Attributes;
using GL.HttpServer.Entities;
using GL.HttpServer.Types;
using Kontur.GameStats.Server.Dto;
using LiteDB;

namespace Kontur.GameStats.Server.Entities
{
    [MapsTo(typeof(MatchDto))]
    public class Match : Entity
    {
        public string Server { get; set; }
        [BsonIndex]
        public DateTimeOffset TimeStamp { get; set; }
        public MatchResult Results { get; set; }
    }
}
