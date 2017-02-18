using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClient
{
    public class ClientResponse
    {
        public ClientResponse()
        {
            
        }

        public string JsonString { get; set; }
        public string ErrorMessage { get; set; }
        public string StatusCode { get; set; }
    }
}
