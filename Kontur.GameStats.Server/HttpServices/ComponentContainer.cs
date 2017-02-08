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
        private List<HttpMethodInfo> _methods = new List<HttpMethodInfo>();
        private List<HttpHandler> _handlers = new List<HttpHandler>();
        private List<KnownTypeParser> _urlParsers = new List<KnownTypeParser>();
        private static ComponentContainer _currentContainer = new ComponentContainer();
        public List<HttpMethodInfo> Methods
        {
            get { return _methods; }
        }

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
                var methods = service.GetMethodsWithAttribute<HttpOperationAttribute>();
                foreach (var method in methods)
                {
                    var attribute = method.GetAttribute<HttpOperationAttribute>();
                    _methods.Add(new HttpMethodInfo(attribute.Name, attribute.Url, method, attribute.MethodType));
                }
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

        public HttpMethodInfo GetMethod(string name, MethodType methodType, List<UrlParameter> urlParameters)
        {
            var methods = _methods.Where(m => m.MethodType == methodType && m.Name == name).ToList();
            if (methods.Count > 1)
            {
               return methods.FirstOrDefault(a => a.MethodInfo.CompareByParams(urlParameters));
            }
            else return methods.FirstOrDefault();
        }
    }
}
