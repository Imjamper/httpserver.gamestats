using Fclp;
using Kontur.GameStats.Server.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Server
{
    public class ServerStarter
    {
        private Configuration _config;

        public ServerStarter(Configuration configuration)
        {
            _config = configuration;
        }

        public void Start()
        {
            using (var server = new StatServer(_config.Prefix))
            {
                server.HandleRequests();
                /*var listeners = server
                        .Where(ctx => ctx.Request.HttpMethod == "GET")
                        .Subscribe(ctx => subject.Take(1)
                                                 .Subscribe(m => ctx.Respond(new StringResponse(m))));

                var publisher = server
                    .Where(ctx => ctx.Request.HttpMethod == "POST")
                    .Subscribe(ctx => ctx.Request.InputStream.ReadBytes(ctx.Request.ContentLength)
                                          .Subscribe(bts =>
                                          {
                                              subject.OnNext(Encoding.UTF8.GetString(bts));
                                              ctx.Respond(new EmptyResponse(201));
                                          }));

                Console.ReadLine();

                listeners.Dispose();
                publisher.Dispose();*/
                Console.WriteLine("Server started. For terminate press any key...");
                Console.ReadLine();
            }
        }
    }
}
