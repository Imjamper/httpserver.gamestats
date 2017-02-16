using GL.HttpServer.Attributes;
using GL.HttpServer.Entities;
using Kontur.GameStats.Server.DTO;

namespace Kontur.GameStats.Server.Entities
{
    [MapsTo(typeof(PlayerScoreDto))]
    public class PlayerScore : Entity
    {
        public string Name { get; set; }

        public int Frags { get; set; }

        public int Kills { get; set; }

        public int Deaths { get; set; }
    }
}