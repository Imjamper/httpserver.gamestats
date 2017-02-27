using System;
using GL.HttpServer.Attributes;
using GL.HttpServer.Entities;
using GL.HttpServer.Types;
using Kontur.GameStats.Server.Dto;

namespace Kontur.GameStats.Server.Entities
{
    [MapsTo(typeof(MatchDto))]
    public class Match : Entity
    {
        public string Server { get; set; }
        public DateOffset TimeStamp { get; set; }
        public MatchResult Results { get; set; }
    }
}
