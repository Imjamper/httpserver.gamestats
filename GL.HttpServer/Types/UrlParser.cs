using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GL.HttpServer.Context;
using GL.HttpServer.HttpServices;
using GL.HttpServer.Extensions;

namespace GL.HttpServer.Types
{
    public class UrlParser
    {
        public static List<UrlParameter> Parse(string url, string methodName, List<HttpMethodInfo> matchMethods, params string[] excludeStrings)
        {
            var parsers = ComponentContainer.Current.GetParsers();
            parsers = parsers.Where(a => 
                matchMethods
                    .SelectMany(b => b.MethodInfo.GetParameters()
                        .Select(p => p.ParameterType))
                            .Any(type => a.Type.IsAssignableFrom(type))).ToList();
            var parameters = new List<UrlParameter>();
            var resultUrl = url;
            foreach (var method in matchMethods)
            {
                foreach(var bindInfo in method.BindInfos)
                {
                    var parser = parsers.FirstOrDefault(p => p.Type.IsAssignableFrom(bindInfo.ParameterType));
                    if (parser != null)
                    {
                        var parameterValue = TryBind(ref resultUrl, bindInfo, parser);
                        parameters.Add(new UrlParameter(parameterValue, bindInfo.ParameterType));
                    }
                }
            }
            foreach (var excludeString in excludeStrings)
            {
                resultUrl = resultUrl.Exclude(excludeString);
            }
            resultUrl = resultUrl.Exclude(methodName); 
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

        private static object TryBind(ref string url, ParameterBindInfo bindInfo, KnownTypeParser parser)
        {
            var bindSegments = bindInfo.MaskSegments;
            if (bindSegments.Count == 2)
            {
                var startSegment = bindSegments[0];
                var endSegment = bindSegments[1];
                var value = url.Extract(startSegment, endSegment).FirstOrDefault();
                if (!value.IsNullOrEmpty())
                {
                    if (parser.CanParse(value))
                    {
                        url = url.Exclude(String.Concat(startSegment, value, endSegment));
                        return parser.ParseObject(value);
                    }
                }
            }
            return null;
        }

        public static bool CompareByParams(MethodInfo method, List<UrlParameter> urlParameters)
        {
            var methodParameters =
                method.GetParameters().Where(p => !typeof(JsonResponse).IsAssignableFrom(p.ParameterType)).ToList();
            if (urlParameters.Count != methodParameters.Count)
                return false;

            for (var index = 0; index <= methodParameters.Count - 1; index++)
            {
                var methodParameter = methodParameters.ElementAt(index);
                var urlParameter = urlParameters.ElementAtOrDefault(index);
                if (methodParameter == null || urlParameter == null || !urlParameter.Type.IsAssignableFrom(methodParameter.ParameterType))
                    return false;
            }

            return true;
        }
    }
}