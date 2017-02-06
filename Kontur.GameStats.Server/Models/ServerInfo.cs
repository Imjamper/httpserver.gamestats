using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Models
{
    public class ServerInfo
    {
        public ServerInfo()
        {

        }

        public string Name { get; set; }

        public List<string> GameModes { get; set; }
    }
}
