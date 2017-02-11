using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace GL.HttpServer.Context
{
    public abstract class Response
    {
        public Response()
        {
            WriteStream = s => { };
            StatusCode = 200;
            Headers = new Dictionary<string, string> {{"Content-Type", "application/json"}};
        }

        [JsonIgnore]
        public int StatusCode { get; set; }

        [JsonIgnore]
        public IDictionary<string, string> Headers { get; set; }

        [JsonIgnore]
        internal Action<Stream> WriteStream { get; set; }
    }
}