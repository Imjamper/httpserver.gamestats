using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Context;

namespace Kontur.GameStats.Server.Models
{
    public class ShortPlayerStats : JsonResponse
    {
        public string Name { get; set; }
        public double KillToDeathRatio { get; set; }
    }
}
