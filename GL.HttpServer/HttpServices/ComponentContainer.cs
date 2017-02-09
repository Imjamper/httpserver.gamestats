using System;
using System.Collections.Generic;
using System.Linq;
using GL.HttpServer.Attributes;
using GL.HttpServer.Extensions;
using GL.HttpServer.Types;

namespace GL.HttpServer.HttpServices
{
    public class ComponentContainer
    {
        private readonly List<HttpHandler> _handlers = new List<HttpHandler>();
        private readonly List<HttpServiceInfo> _httpServices = new List<HttpServiceInfo>();
        private readonly List<KnownTypeParser> _urlParsers = new List<KnownTypeParser>();

        public ComponentContainer()
        {
            Initialize();
        }

        public static ComponentContainer Current { get; } = new ComponentContainer();

        public void Initialize()
        {
            var httpServiceType = typeof(IHttpService);
            var services = GetType()
                .Assembly
                .GetTypes()
                .Where(p => httpServiceType.IsAssignableFrom(p) && p.IsClass);
            foreach (var service in services)
            {
                var serviceName = string.Empty;
                var httpServiceAttribute = service.GetAttribute<HttpServiceAttribute>();
                if (httpServiceAttribute != null)
                    serviceName = httpServiceAttribute.Name;
                else serviceName = service.Name.Replace("Service", string.Empty).ToLower();
                var methods = service.GetMethodsWithAttribute<HttpOperationAttribute>();
                var serviceInfo = new HttpServiceInfo(serviceName, methods);
                _httpServices.Add(serviceInfo);
            }

            var httpHandlerType = typeof(HttpHandler);

            var handlerTypes = GetType()
                .Assembly
                .GetTypes()
                .Where(p => httpHandlerType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToList();
            foreach (var handlerType in handlerTypes)
            {
                var handlerInstance = Activator.CreateInstance(handlerType) as HttpHandler;
                handlerInstance.SetContainer(this);
                _handlers.Add(handlerInstance);
            }

            var descriptorInterface = typeof(IKnownTypeParser);

            var descriptorsTypes = GetType()
                .Assembly
                .GetTypes()
                .Where(p => descriptorInterface.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToList();
            foreach (var descriptorType in descriptorsTypes)
                _urlParsers.Add(Activator.CreateInstance(descriptorType) as KnownTypeParser);
        }

        public List<HttpHandler> GetHandlers()
        {
            return _handlers;
        }

        public List<KnownTypeParser> GetParsers()
        {
            return _urlParsers;
        }

        public HttpServiceInfo GetService(string serviceName)
        {
            return _httpServices.FirstOrDefault(a => a.Name == serviceName);
        }
    }
}