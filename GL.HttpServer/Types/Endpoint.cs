using System;
using System.Linq;
using System.Net;

namespace GL.HttpServer.Types
{
    public class Endpoint
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public static bool TryParse(string endpointstring, out Endpoint endpoint)
        {
            if (!endpointstring.Contains('-'))
            {
                endpoint = null;
                return false;
            }

            Uri url;
            IPAddress ip;
            if (Uri.TryCreate($"http://{endpointstring.Replace('-', ':')}", UriKind.Absolute, out url))
                if (IPAddress.TryParse(url.Host, out ip))
                {
                    endpoint = new Endpoint {Host = ip.ToString(), Port = url.Port};
                    return true;
                }
                else
                {
                    endpoint = new Endpoint {Host = url.Host, Port = url.Port};
                    return true;
                }
            endpoint = null;
            return false;
        }

        public override string ToString()
        {
            return $"{Host}-{Port}";
        }
    }
}