using Kontur.GameStats.Server.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Context;
using Kontur.GameStats.Server.Extensions;

namespace Kontur.GameStats.Server.HttpServices
{
    public class PutHttpHandler : HttpHandler
    {
        public PutHttpHandler()
        {
            MethodType = MethodType.PUT;
        }

        public override void ProcessRequest(RequestContext requestContext)
        {
            requestContext.Request.InputStream.ReadBytes(requestContext.Request.ContentLength)
                .Subscribe(new Action<byte[]>(a => 
                {
                    var request = Encoding.UTF8.GetString(a);
                    requestContext.Respond(new StringResponse("запрос получен и обработан"));
                }));
        }
    }
}
