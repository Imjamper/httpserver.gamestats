using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Extensions;
using Kontur.GameStats.Server.Attributes;
using Kontur.GameStats.Server.Enums;

namespace Kontur.GameStats.Server.HttpServices
{
    public class ServicesContainer
    {
        private List<MethodInfoItem> _methods = new List<MethodInfoItem>();
        private List<HttpHandler> _handlers = new List<HttpHandler>();
        private static ServicesContainer _currentContainer = new ServicesContainer();
        public List<MethodInfoItem> Methods
        {
            get { return _methods; }
        }

        public ServicesContainer()
        {
            Initialize();
        }

        public static ServicesContainer Current
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
                    _methods.Add(new MethodInfoItem(attribute.Url, method, attribute.MethodType));
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
        }

        public List<HttpHandler> GetHandlers()
        {
            return _handlers;
        }

        public MethodInfoItem GetMethod(string name, MethodType methodType)
        {
            return _methods.FirstOrDefault(m => m.MethodType == methodType && m.Name == name);
        }
    }
}
