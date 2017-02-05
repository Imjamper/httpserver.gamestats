using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Extensions
{
    public static class StreamExtensions
    {
        public static IObservable<byte[]> ReadBytes(this Stream stream, int count)
        {
            var buffer = new byte[count];
            return Observable.FromAsyncPattern((cb, state) => stream.BeginRead(buffer, 0, count, cb, state), ar =>
            {
                stream.EndRead(ar);
                return buffer;
            })();
        }
    }
}
