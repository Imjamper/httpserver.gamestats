using System;
using GL.HttpServer.Attributes;
using GL.HttpServer.Entities;
using Kontur.GameStats.Server.Dto;

namespace Kontur.GameStats.Server.Entities
{
    [MapsTo(typeof(MatchDto))]
    public class Match : Entity
    {
        public string Server { get; set; }
        public DateTime? TimeStamp { get; set; }
        public MatchResult Results { get; set; }
    }
}
