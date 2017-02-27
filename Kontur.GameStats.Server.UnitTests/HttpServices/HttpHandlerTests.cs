using System.Linq;
using System.Reactive.Linq;
using GL.HttpServer.Context;
using GL.HttpServer.Enums;
using GL.HttpServer.Types;
using NUnit.Framework;

namespace Kontur.GameStats.Server.UnitTests.HttpServices
{
    [TestFixture]
    public class HttpHandlerTests
    {
        [Test]
        public void ProcessGetRequest_NewGetRequest_GetValidMethod()
        {
            ComponentContainer.Current.Initialize();
            var getHandler = ComponentContainer.Current.GetHandlers().FirstOrDefault(a => a.MethodType == MethodType.GET);

            Assert.IsNotNull(getHandler);
            
            var request = new Request
            {
                HttpMethod = MethodType.GET,
                UnescapedUrl = "/test/localhost-9999/GetMethod[/5]"
            };
            var requestContext = new RequestContext(request);
            var method = getHandler.GetMethod(requestContext).Wait();
            Assert.AreEqual(requestContext.Request.Parameters.Count, 2);
            Assert.IsNotNull(method);
        }

        [Test]
        public void ProcessPutRequest_NewPutRequest_GetValidMethod()
        {
            ComponentContainer.Current.Initialize();
            var putHandler = ComponentContainer.Current.GetHandlers().FirstOrDefault(a => a.MethodType == MethodType.PUT);

            Assert.IsNotNull(putHandler);

            var request = new Request
            {
                HttpMethod = MethodType.PUT,
                UnescapedUrl = "/test/localhost-9999/PutMethod"
            };
            var requestContext = new RequestContext(request);
            var method = putHandler.GetMethod(requestContext).Wait();
            Assert.AreEqual(requestContext.Request.Parameters.Count, 1);
            Assert.IsNotNull(method);
        }
    }
}
