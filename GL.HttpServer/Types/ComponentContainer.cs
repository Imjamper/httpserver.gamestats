using System;
using System.Collections.Generic;
using System.Linq;
using GL.HttpServer.Attributes;
using GL.HttpServer.Cache;
using GL.HttpServer.Extensions;
using GL.HttpServer.HttpServices;

namespace GL.HttpServer.Types
{
    public class ComponentContainer
    {
        private readonly List<HttpHandler> _handlers = new List<HttpHandler>();
        private readonly List<HttpServiceInfo> _httpServices = new List<HttpServiceInfo>();
        private readonly List<KnownTypeParser> _urlParsers = new List<KnownTypeParser>();
        private static ComponentContainer _currentContainer;

        public ComponentContainer()
        {
        }

        public static ComponentContainer Current
        {
            get
            {
                if (_currentContainer == null)
                {
                    _currentContainer = new ComponentContainer();
                }
                    
                return _currentContainer;
            }
        }

        public void Initialize()
        {
            var httpServiceType = typeof(IHttpService);
            var services = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(p => httpServiceType.IsAssignableFrom(p) && p.IsClass).ToList();
            if (services.Any())
                Console.WriteLine("The available methods of the server:");
            foreach (var service in services)
            {
                var httpServiceAttribute = service.GetAttribute<HttpServiceAttribute>();
                var serviceName = httpServiceAttribute != null ? httpServiceAttribute.Name : service.Name.Replace("Service", string.Empty).ToLower();
                var methods = service.GetMethodsWithAttribute<HttpOperationAttribute>();
                var serviceInfo = new HttpServiceInfo(serviceName, methods);
                _httpServices.Add(serviceInfo);
            }

            var httpHandlerType = typeof(IHttpHandler);

            var handlerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(p => httpHandlerType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToList();
            foreach (var handlerType in handlerTypes)
            {
                var handlerInstance = InstanceActivator.CreateInstance(handlerType) as HttpHandler;
                _handlers.Add(handlerInstance);
            }

            var descriptorInterface = typeof(IKnownTypeParser);

            var descriptorsTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(p => descriptorInterface.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToList();
            foreach (var descriptorType in descriptorsTypes)
            {
                var parser = InstanceActivator.CreateInstance(descriptorType) as KnownTypeParser;
                _urlParsers.Add(parser);
            }

            var cacheLoaderInterface = typeof(ICacheLoader);

            var cacheLoaderTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(p => cacheLoaderInterface.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToList();
            foreach (var cacheLoaderType in cacheLoaderTypes)
            {
                var loader = InstanceActivator.CreateInstance(cacheLoaderType) as ICacheLoader;
                loader?.Execute();
            }
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