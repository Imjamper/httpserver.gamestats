using System;
using System.Linq;

namespace GL.HttpServer.Extensions
{
    public static class StringExtensions
    {
        public static string GetServiceName(this string url)
        {
            return url.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .FirstOrDefault();
        }

        public static string Exclude(this string baseString, string excludeString)
        {
            return baseString.Replace(excludeString, string.Empty);
        }
    }
}