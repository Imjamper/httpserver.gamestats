using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Context;
using LiteDB;

namespace Kontur.GameStats.Server.Models
{
    public class FullServerInfo : JsonResponse
    {
        public FullServerInfo()
        {
            Info = new ServerInfo();
        }

        [BsonIndex(true)]
        public string Endpoint { get; set; }
        
        public ServerInfo Info { get; set; }
    }
}
