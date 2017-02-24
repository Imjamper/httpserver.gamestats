using System.Diagnostics;

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
