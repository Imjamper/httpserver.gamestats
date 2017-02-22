namespace GL.HttpServer.Context
{
    public class EmptyResponse : Response
    {
        public EmptyResponse(int statusCode = 404)
        {
            StatusCode = statusCode;
        }
    }
}
