using GL.HttpServer.Enums;

namespace GL.HttpServer.HttpServices
{
    public class PutHttpHandler : HttpHandler
    {
        public PutHttpHandler()
        {
            MethodType = MethodType.PUT;
        }
    }
}