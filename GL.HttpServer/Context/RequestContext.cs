using System;
using System.Linq;
using System.Net;
using System.Text;
using GL.HttpServer.Enums;
using GL.HttpServer.Extensions;

namespace GL.HttpServer.Context
{
    public class RequestContext
    {
        private readonly HttpListenerResponse _listenerResponse;

        public RequestContext(HttpListenerRequest request, HttpListenerResponse response)
        {
            _listenerResponse = response;
            Request = MapRequest(request);
        }

        public Request Request { get; }

        private Request MapRequest(HttpListenerRequest request)
        {
            var mapRequest = new Request
            {
                Headers = request.Headers.ToDictionary(),
                HttpMethod = request.HttpMethod.FromString(),
                InputStream = request.InputStream,
                RawUrl = request.RawUrl,
                UnescapedUrl = Uri.UnescapeDataString(request.RawUrl)
            };
            return mapRequest;
        }

        public void Respond(Response response)
        {
            foreach (var header in response.Headers.Where(r => r.Key != "Content-Type"))
                _listenerResponse.AddHeader(header.Key, header.Value);

            _listenerResponse.ContentType = response.Headers["Content-Type"];
            _listenerResponse.StatusCode = response.StatusCode;
            _listenerResponse.ContentEncoding = Encoding.UTF8;

            using (var output = _listenerResponse.OutputStream)
            {
                response.WriteStream(output);
            }
        }
    }
}