using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClient
{
    public class ClientResponse
    {
        public ClientResponse()
        {
            Stopwatch = new Stopwatch();
        }

        public Stopwatch Stopwatch { get; set; }

        public string JsonString { get; set; }
        public string ErrorMessage { get; set; }
        public string StatusCode { get; set; }
    }
}
