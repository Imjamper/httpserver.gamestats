using Kontur.GameStats.Server.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Context
{
    public class Request
    {
        public MethodType HttpMethod { get; set; }
        public IDictionary<string, IEnumerable<string>> Headers { get; set; }
        public Stream InputStream { get; set; }
        public string RawUrl { get; set; }
        public int ContentLength
        {
            get { return int.Parse(Headers["Content-Length"].First()); }
        }
    }
}
