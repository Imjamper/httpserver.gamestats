using System;
using GL.HttpServer.Context;
using GL.HttpServer.Enums;

namespace GL.HttpServer.HttpServices
{
    public interface IHttpHandler
    {
        MethodType MethodType { get; set; }

        void Subscribe(IObservable<RequestContext> observableContext);

        void ProcessRequest(RequestContext requestContext);

        IObservable<HttpMethodInfo> GetMethod(RequestContext requestContext);

        void SetContainer(ComponentContainer servicesContainer);
    }
}