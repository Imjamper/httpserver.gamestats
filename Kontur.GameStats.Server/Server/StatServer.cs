using Kontur.GameStats.Server.Context;
using Kontur.GameStats.Server.HttpServices;
using System;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Extensions;
using System.Text;

namespace Kontur.GameStats.Server
{
    public class StatServer : IObservable<RequestContext>, IDisposable
    {
        private readonly HttpListener listener;
        private readonly IObservable<RequestContext> stream;

        public StatServer(string url)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            stream = ObservableHttpContext();
        }

        private IObservable<RequestContext> ObservableHttpContext()
        {
            return Observable.Create<RequestContext>(obs =>Observable.FromAsyncPattern(listener.BeginGetContext, listener.EndGetContext)()
                                          .Select(c => new RequestContext(c.Request, c.Response))
                                          .Subscribe(obs))
                             .Repeat()
                             .Retry()
                             .Publish()
                             .RefCount();
        }
        public void Dispose()
        {
            listener.Stop();
        }

        public IDisposable Subscribe(IObserver<RequestContext> observer)
        {
            return stream.Subscribe(observer);
        }

        public void HandleRequests()
        {
            var handlers = ServicesContainer.Current.GetHandlers();
            handlers.ForEach(h => h.Subscribe(this));
        }
    }
}