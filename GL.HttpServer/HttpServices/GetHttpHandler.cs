using GL.HttpServer.Enums;

namespace GL.HttpServer.HttpServices
{
    public class GetHttpHandler : HttpHandler
    {
        public GetHttpHandler()
        {
            MethodType = MethodType.GET;
        }
    }
}