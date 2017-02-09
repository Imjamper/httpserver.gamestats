using System;
using System.Net;
using GL.HttpServer.Context;
using GL.HttpServer.HttpServices;

namespace GL.HttpServer.Server
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
                    .ObserveOn(NewThreadScheduler.Default)
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