using System;
using System.Net;
using GL.HttpServer.Context;
using GL.HttpServer.HttpServices;
using System.Reactive.Linq;
using System.Reactive.Concurrency;

namespace GL.HttpServer.Server
{
    public class HttpServer : IObservable<RequestContext>, IDisposable
    {
        private readonly HttpListener listener;
        private readonly IObservable<RequestContext> stream;

        public HttpServer(string url)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            stream = ObservableHttpContext();
            SubscribeHandlers();
        }

        public void Dispose()
        {
            listener.Stop();
        }

        public IDisposable Subscribe(IObserver<RequestContext> observer)
        {
            return stream.Subscribe(observer);
        }

        private IObservable<RequestContext> ObservableHttpContext()
        {
            return Observable.Create<RequestContext>(obs => Observable.FromAsync(() => listener.GetContextAsync())
                    .Select(c => new RequestContext(c.Request, c.Response))
                    .ObserveOn(ThreadPoolScheduler.Instance)
                    .Subscribe(obs))
                .Repeat()
                .Retry()
                .Publish()
                .RefCount();
        }

        private void SubscribeHandlers()
        {
            var handlers = ComponentContainer.Current.GetHandlers();
            handlers.ForEach(h => h.Subscribe(this));
        }
    }
}