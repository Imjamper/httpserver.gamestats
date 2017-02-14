using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Entities;

namespace Kontur.GameStats.Server.Entities
{
    public class Match : Entity
    {
        public string Server { get; set; }
        public DateTime TimeStamp { get; set; }
        public MatchResult Results { get; set; }
    }
}
