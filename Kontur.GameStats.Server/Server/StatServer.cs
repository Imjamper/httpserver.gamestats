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
using System.Reactive.Concurrency;

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
            SubscribeHandlers();
        }

        private IObservable<RequestContext> ObservableHttpContext()
        {
            return Observable.Create<RequestContext>(obs => Observable.FromAsync(()=> listener.GetContextAsync())
                                          .Select(c => new RequestContext(c.Request, c.Response))
                                          .ObserveOn(NewThreadScheduler.Default)
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

        private void SubscribeHandlers()
        {
            var handlers = ComponentContainer.Current.GetHandlers();
            handlers.ForEach(h => h.Subscribe(this));
        }
    }
}