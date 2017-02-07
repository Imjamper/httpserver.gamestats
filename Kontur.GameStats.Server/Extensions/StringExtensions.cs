using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Extensions
{
    public static class StringExtensions
    {
        public static char[] UrlParametersDelimiters = new char[] {'<'};

        public static int GetIndexInUrl(this string baseString, string subString)
        {
            return baseString.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList().FindIndex(s => s.Contains(subString) && s.Contains("<"));
        }
        
        public static string GetMethodName(this string url)
        {
            return url.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .LastOrDefault(a => !a.Contains("<"));
        }

        public static string GetValueByIndex(this string url, int index)
        {
            return url.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .ElementAt(index);
        }

        public static bool CompareUrlBySegments(this string baseString, string stringToCompare)
        {
            var firstLength = baseString.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length;
            var secondLength = stringToCompare.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length;

            return firstLength == secondLength;
        } 
    }
}
