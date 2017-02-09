using System;
using System.IO;
using System.Threading.Tasks;

namespace GL.HttpServer.Extensions
{
    public static class StreamExtensions
    {
        public static IObservable<byte[]> ReadBytes(this Stream stream, int count)
        {
            var buffer = new byte[count];
            return Observable.FromAsync(() =>
            {
                return Task.Factory.StartNew(() =>
                {
                    var index = stream.ReadAsync(buffer, 0, count);
                    return buffer;
                });
            });
        }
    }
}