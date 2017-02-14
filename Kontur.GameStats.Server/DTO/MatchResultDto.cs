using System.Collections.Generic;
using GL.HttpServer.Context;
using Kontur.GameStats.Server.DTO;

namespace Kontur.GameStats.Server.Dto
{
    public class MatchResultDto : JsonResponse
    {
        public MatchResultDto()
        {
            ScoreBoard = new List<PlayerScoreDto>();
        }

        public string Map { get; set; }
        public string GameMode { get; set; }
        public int FragLimit { get; set; }
        public int TimeLimit { get; set; }
        public double TimeElapsed { get; set; }
        public List<PlayerScoreDto> ScoreBoard { get; set; }
    }
}