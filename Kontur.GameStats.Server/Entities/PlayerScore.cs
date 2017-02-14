using GL.HttpServer.Entities;

namespace Kontur.GameStats.Server.Entities
{
    public class PlayerScore : Entity
    {
        public string Name { get; set; }

        public int Frags { get; set; }

        public int Kills { get; set; }

        public int Deaths { get; set; }
    }
}