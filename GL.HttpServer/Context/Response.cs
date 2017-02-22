using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Newtonsoft.Json;

namespace GL.HttpServer.Context
{
    /// <summary>
    /// Абстрактный класc для HttpResponse
    /// </summary>
    public abstract class Response
    {
        protected Response()
        {
            WriteStream = s => { };
            StatusCode = 200;
            Headers = new Dictionary<string, string> {{"Content-Type", "application/json"}};
        }

        [IgnoreMap]
        [JsonIgnore]
        public int StatusCode { get; set; }

        [IgnoreMap]
        [JsonIgnore]
        public IDictionary<string, string> Headers { get; set; }

        [IgnoreMap]
        [JsonIgnore]
        internal Action<Stream> WriteStream { get; set; }
    }
}