using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Cache;
using GL.HttpServer.Database;
using Kontur.GameStats.Server.Dto.CacheInfo;
using Kontur.GameStats.Server.Entities;

namespace Kontur.GameStats.Server.CacheLoaders
{
    public class RecentMatchesCacheLoader : ICacheLoader
    {
        public const string RecentMatchesUid = "f887dc1d-fa9e-4db4-b858-9afc45ad65b3";
        public void Execute()
        {
            using (var unit = new UnitOfWork(true))
            {
                var matches = unit.Repository<Match>().FindAll();
                var byTimestamp = matches.OrderByDescending(a => a.TimeStamp).Take(50).ToList();

                MemoryCache.Cache<RecentMatchesTempInfo>().AddOrUpdate(RecentMatchesUid, new RecentMatchesTempInfo(byTimestamp));
            }
        }
    }
}
