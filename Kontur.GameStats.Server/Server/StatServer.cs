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
        private ServicesContainer _servicesContainer;

        public StatServer(string url)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            stream = ObservableHttpContext();
            _servicesContainer = new ServicesContainer();
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
            this.Subscribe(async =>
            {
                var a1 = async.Request.InputStream.ReadBytes(async.Request.ContentLength).Subscribe(a => Encoding.UTF8.GetString(a));
                var b2 = a1;
            });
        }
    }
}