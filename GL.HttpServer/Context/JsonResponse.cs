using System.Text;
using System.IO;
using GL.HttpServer.Dto;
using Newtonsoft.Json;

namespace GL.HttpServer.Context
{
    /// <summary>
    /// Ответ содержащий Json
    /// </summary>
    public class JsonResponse : Response, IDto
    {
        public JsonResponse(string json)
        {
            var bytes = Encoding.UTF8.GetBytes(json);
            WriteStream = s => s.Write(bytes, 0, bytes.Length);
        }

        public JsonResponse()
        {
            WriteStream = WriteJson;
        }

        protected virtual void WriteJson(Stream stream)
        {
            var json = JsonConvert.SerializeObject(this);
            var bytes = Encoding.UTF8.GetBytes(json);
            Headers.Add("Content-Length", json.Length.ToString());
            stream.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
