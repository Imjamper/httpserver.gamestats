using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Context
{
    public class Response
    {
        public Response()
        {
            WriteStream = s => { };
            StatusCode = 200;
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } };
        }

        [JsonIgnore]
        public int StatusCode { get; set; }
        [JsonIgnore]
        public IDictionary<string, string> Headers { get; set; }
        [JsonIgnore]
        public Action<Stream> WriteStream { get; set; }
    }

    public class JsonResponse : Response
    {
        public JsonResponse(string json) : base()
        {
            var bytes = Encoding.UTF8.GetBytes(json);
            WriteStream = s => s.Write(bytes, 0, bytes.Length);
        }

        public JsonResponse()
        {
            WriteStream = WriteJson;
        }

        private void WriteJson(Stream stream)
        {
            var json = JsonConvert.SerializeObject(this);
            var bytes = Encoding.UTF8.GetBytes(json);
            Headers.Add("Content-Length", json.Length.ToString());
            stream.WriteAsync(bytes, 0, bytes.Length);
        }
    }

    public class EmptyResponse : Response
    {
        public EmptyResponse(int statusCode = 404)
        {
            StatusCode = statusCode;
        }
    }
}
