using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace GL.HttpServer.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ReadAll(this Stream stream, int count)
        {
            var buffer = new byte[count];
            return Task.Factory.StartNew(() =>
            {
                stream.ReadAsync(buffer, 0, count);
                return buffer;
            }).GetAwaiter().GetResult();
        }
    }
}