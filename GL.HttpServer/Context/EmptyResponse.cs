using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.HttpServer.Context
{
    public class EmptyResponse : Response
    {
        public EmptyResponse(int statusCode = 404)
        {
            StatusCode = statusCode;
        }
    }
}
