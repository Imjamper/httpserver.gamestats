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
            get
            {
                int length = 0;
                IEnumerable<string> contentLengthString;
                if (Headers.TryGetValue("Content-Length", out contentLengthString))
                {
                    var firstValue = contentLengthString.FirstOrDefault();
                    if (firstValue != null)
                    {
                        int.TryParse(firstValue, out length);
                    }
                }
                return length;
            }
        }
    }
}
