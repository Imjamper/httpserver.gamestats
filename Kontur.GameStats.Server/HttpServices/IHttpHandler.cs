using Kontur.GameStats.Server.Context;
using Kontur.GameStats.Server.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.HttpServices
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
