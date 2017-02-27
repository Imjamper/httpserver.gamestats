using GL.HttpServer.Attributes;
using GL.HttpServer.HttpServices;
using GL.HttpServer.Types;

namespace Kontur.GameStats.Server.UnitTests.TestModels
{
    [HttpService("test")]
    public class TestService : IHttpService
    {
        [GetOperation("GetMethod", "/<endpoint>/GetMethod[/<count>]")]
        public void TestGetMethod([Bind("[/{count}]")]int count, Endpoint endpoint)
        {
            
        }

        [PutOperation("PutMethod", "/<endpoint>/PutMethod")]
        public void TestPutMethod(Endpoint endpoint)
        {

        }
    }
}
