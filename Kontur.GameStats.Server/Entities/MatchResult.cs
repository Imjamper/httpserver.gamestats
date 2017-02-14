using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.Server.DTO;

namespace Kontur.GameStats.Server.Entities
{
    public class MatchResult
    {
        public MatchResult()
        {
            ScoreBoard = new List<PlayerScore>();
        }

        public string Map { get; set; }
        public string GameMode { get; set; }
        public int FragLimit { get; set; }
        public int TimeLimit { get; set; }
        public double TimeElapsed { get; set; }
        public List<PlayerScore> ScoreBoard { get; set; }
    }
}
