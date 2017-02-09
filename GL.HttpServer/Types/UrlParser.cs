using System;
using System.Collections.Generic;
using System.Linq;
using GL.HttpServer.HttpServices;

namespace GL.HttpServer.Types
{
    public class UrlParser
    {
        public static List<UrlParameter> Parse(string url, params string[] excludeStrings)
        {
            var parsers = ComponentContainer.Current.GetParsers();
            var parameters = new List<UrlParameter>();
            var resultUrl = url;
            foreach (var excludeString in excludeStrings)
                resultUrl = resultUrl.Replace(excludeString, string.Empty);
            var segments = resultUrl
                .Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Where(a => !string.IsNullOrEmpty(a) && !string.IsNullOrWhiteSpace(a))
                .ToList();
            var parsedSegments = new List<string>();
            foreach (var segment in segments)
            {
                var descriptor = parsers.FirstOrDefault(a => a.CanParse(segment));
                if (descriptor != null)
                {
                    parsedSegments.Add(segment);
                    var value = descriptor.ParseObject(segment);
                    var type = descriptor.Type;
                    parameters.Add(new UrlParameter(value, type));
                }
            }
            segments.RemoveAll(a => parsedSegments.Contains(a));
            foreach (var segment in segments)
                parameters.Add(new UrlParameter(segment, typeof(string)));
            return parameters;
        }

        public static bool IsKnownParameter(string segment)
        {
            var parsers = ComponentContainer.Current.GetParsers();
            return parsers.Any(a => a.CanParse(segment));
        }
    }
}