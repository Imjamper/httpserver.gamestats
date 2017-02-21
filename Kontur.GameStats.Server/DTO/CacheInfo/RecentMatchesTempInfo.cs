using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Context;
using GL.HttpServer.Extensions;
using Kontur.GameStats.Server.Entities;

namespace Kontur.GameStats.Server.Dto.CacheInfo
{
    public class RecentMatchesTempInfo
    {
        private List<MatchDto> _recentMatches;
        public RecentMatchesTempInfo()
        {
            _recentMatches = new List<MatchDto>();
        }

        public RecentMatchesTempInfo(List<Match> matches)
        {
            _recentMatches = new List<MatchDto>();
            foreach (var match in matches)
            {
                Add(match);
            }
        }

        public int Count => _recentMatches.Count;

        public JsonList<MatchDto> Take(int count)
        {
            return _recentMatches.Take(count).ToJsonList();
        }

        public void Add(Match match)
        {
            if (_recentMatches.Count == 50)
            {
                _recentMatches.RemoveAt(49);
            }
            _recentMatches.Insert(0, match.ToDto<MatchDto>());
            _recentMatches = _recentMatches.OrderByDescending(a => a.TimeStamp).ToList();
        }

        public JsonList<MatchDto> GetAll()
        {
            return _recentMatches.ToJsonList();
        }
    }
}
