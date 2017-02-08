using Kontur.GameStats.Server.HttpServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Types
{
    public class UrlParser  
    {
        public static List<UrlParameter> Parse(string url)
        {
            var parsers = ComponentContainer.Current.GetParsers();
            var parameters = new List<UrlParameter>();
            var segments = url.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var segment in segments)
            {
                var descriptor = parsers.FirstOrDefault(a => a.CanParse(segment));
                if (descriptor != null)
                {
                    var value = descriptor.ParseObject(segment);
                    var type = descriptor.Type;
                    parameters.Add(new UrlParameter(value, type));
                }
            }

            return parameters;
        }

        public static bool IsKnownParameter(string segment)
        {
            var parsers = ComponentContainer.Current.GetParsers();
            return parsers.Any(a => a.CanParse(segment));
        }
    }
}
