using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Context;

namespace GL.HttpServer.Exceptions
{
    public class MethodNotFoundException : Exception
    {
        public MethodNotFoundException(RequestContext requestContext) 
            : base($"The method is not found. Invalid request. Request: {requestContext.Request.HttpMethod:G} {requestContext.Request.RawUrl}")
        {
            
        }
    }
}
