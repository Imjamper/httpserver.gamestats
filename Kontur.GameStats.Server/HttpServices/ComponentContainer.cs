using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Extensions;
using Kontur.GameStats.Server.Attributes;
using Kontur.GameStats.Server.Enums;
using Kontur.GameStats.Server.Types;

namespace Kontur.GameStats.Server.HttpServices
{
    public class ComponentContainer
    {
        private List<HttpServiceInfo> _httpServices = new List<HttpServiceInfo>();
        private List<HttpHandler> _handlers = new List<HttpHandler>();
        private List<KnownTypeParser> _urlParsers = new List<KnownTypeParser>();
        private static ComponentContainer _currentContainer = new ComponentContainer();

        public ComponentContainer()
        {
            Initialize();
        }

        public static ComponentContainer Current
        {
            get { return _currentContainer; }
        }

        public void Initialize()
        {
            var httpServiceType = typeof(IHttpService);
            var services = this.GetType()
                .Assembly
                .GetTypes()
                .Where(p => httpServiceType.IsAssignableFrom(p) && p.IsClass);
            foreach (var service in services)
            {
                string serviceName = String.Empty;
                var httpServiceAttribute = service.GetAttribute<HttpServiceAttribute>();
                if (httpServiceAttribute != null)
                    serviceName = httpServiceAttribute.Name;
                else serviceName = service.Name.Replace("Service", String.Empty).ToLower();   
                var methods = service.GetMethodsWithAttribute<HttpOperationAttribute>();
                var serviceInfo = new HttpServiceInfo(serviceName, methods);
                _httpServices.Add(serviceInfo);
            }

            var httpHandlerType = typeof(HttpHandler);

            var handlerTypes = this.GetType()
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
            {
                _urlParsers.Add(Activator.CreateInstance(descriptorType) as KnownTypeParser);
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
