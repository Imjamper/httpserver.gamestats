using System;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using GL.HttpServer.Context;
using GL.HttpServer.Types;

namespace GL.HttpServer
{
    public class HttpServer : IObservable<RequestContext>, IDisposable
    {
        private readonly HttpListener _listener;
        private IObservable<RequestContext> _stream;

        public HttpServer()
        {
            _listener = new HttpListener();
        }

        public void Start(string url)
        {
            if (!_listener.Prefixes.Contains(url))
            {
                _listener.Prefixes.Add(url);
                _listener.Start();
                _stream = ObservableHttpContext();
                SubscribeHandlers();
            }
        }

        public void Dispose()
        {
            try
            {
                _listener.Stop();
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public IDisposable Subscribe(IObserver<RequestContext> observer)
        {
            return _stream.Subscribe(observer);
        }

        private IObservable<RequestContext> ObservableHttpContext()
        {
            return Observable.Create<RequestContext>(obs => Observable.FromAsync(() => _listener.GetContextAsync())
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