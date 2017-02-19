using System;
using GL.HttpServer.Context;
using GL.HttpServer.Enums;
using GL.HttpServer.Types;

namespace GL.HttpServer.HttpServices
{
    public interface IHttpHandler
    {
        ComponentContainer ComponentContainer { get;}
        MethodType MethodType { get; set; }

        void Subscribe(IObservable<RequestContext> observableContext);

        void ProcessRequest(RequestContext requestContext);

        IObservable<HttpMethodInfo> GetMethod(RequestContext requestContext);
    }
}