using Fclp;
using Kontur.GameStats.Server.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Server
{
    public class ServerStarter
    {
        private Configuration _config;

        public ServerStarter(Configuration configuration)
        {
            _config = configuration;
        }

        public void Start()
        {
            using (var server = new StatServer(_config.Prefix))
            {
                Console.WriteLine("Server started. For terminate press any key...");
                Console.ReadLine();
            }
        }
    }
}
