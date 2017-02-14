using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GL.HttpServer.Entities;
using LiteDB;
using Newtonsoft.Json;

namespace GL.HttpServer.Context
{
    public abstract class Response : IEntity
    {
        protected Response()
        {
            WriteStream = s => { };
            StatusCode = 200;
            Headers = new Dictionary<string, string> {{"Content-Type", "application/json"}};
        }

        [BsonIgnore]
        [JsonIgnore]
        public int StatusCode { get; set; }

        [BsonIgnore]
        [JsonIgnore]
        public IDictionary<string, string> Headers { get; set; }

        [BsonIgnore]
        [JsonIgnore]
        internal Action<Stream> WriteStream { get; set; }

        [JsonIgnore]
        public int Id { get; set; }
    }
}