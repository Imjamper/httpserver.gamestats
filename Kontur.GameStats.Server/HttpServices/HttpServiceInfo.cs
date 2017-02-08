using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.HttpServices
{
    public class HttpServiceInfo
    {
        public string Name { get; set; }

        public List<HttpMethodInfo> Methods { get; set; }


    }
}
