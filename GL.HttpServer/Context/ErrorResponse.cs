using GL.HttpServer.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.HttpServer.Context
{
    public class ErrorResponse : JsonResponse
    {
        public ErrorResponse()
        {
            StatusCode = 500;
        }

        public ErrorResponse(string errorMessage, int statusCode = 500) : base(errorMessage)
        {
            StatusCode = statusCode;
        }

        public string ErrorMessage { get; set; }

        protected override void WriteJson(Stream stream)
        {
            var error = ErrorMessage;
            if (!error.IsNullOrEmpty())
            {
                var bytes = Encoding.UTF8.GetBytes(ErrorMessage);
                Headers.Add("Content-Length", error.Length.ToString());
                stream.WriteAsync(bytes, 0, bytes.Length);
            }
        }
    }
}
