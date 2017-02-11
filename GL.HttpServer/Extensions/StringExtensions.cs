using System;
using System.Collections.Generic;
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

        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }

        public static List<string> Extract(this string text, string startString, string endString)
        {
            List<string> matched = new List<string>();
            int indexStart = 0, indexEnd = 0;
            bool exit = false;
            while (!exit)
            {
                indexStart = text.IndexOf(startString);
                indexEnd = text.IndexOf(endString);
                if (indexStart != -1 && indexEnd != -1)
                {
                    matched.Add(text.Substring(indexStart + startString.Length,
                        indexEnd - indexStart - startString.Length));
                    text = text.Substring(indexEnd + endString.Length);
                }
                else
                    exit = true;
            }
            return matched;
        }
    }
}