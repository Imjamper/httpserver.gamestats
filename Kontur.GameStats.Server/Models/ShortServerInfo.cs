using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Context;

namespace Kontur.GameStats.Server.Models
{
    public class ShortServerInfo : JsonResponse
    {
        public double AverageMatchesPerDay { get; set; }
        public string Endpoint { get; set; }
        public string Name { get; set; }
    }
}
