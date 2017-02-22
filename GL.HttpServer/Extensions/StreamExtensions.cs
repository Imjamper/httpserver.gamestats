using System.IO;

namespace GL.HttpServer.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ReadAll(this Stream stream, int count)
        {
            byte[] buffer = new byte[count];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}