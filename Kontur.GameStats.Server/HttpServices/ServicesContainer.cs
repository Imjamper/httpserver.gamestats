using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Extensions;
using Kontur.GameStats.Server.Attributes;

namespace Kontur.GameStats.Server.HttpServices
{
    public class ServicesContainer
    {
        private List<MethodStoreItem> _methodsItem = new List<MethodStoreItem>();
        public ServicesContainer()
        {
            Initialize();
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
                    _methodsItem.Add(new MethodStoreItem(attribute.Url, method, attribute.MethodType));
                }
            }
        }
    }
}
