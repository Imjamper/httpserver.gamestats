using Kontur.GameStats.Server.Models;
using Kontur.GameStats.Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Extensions
{
    public static class StringExtensions
    {
        public static string GetMethodName(this string url)
        {
            return url.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .LastOrDefault(s => !UrlParser.IsKnownParameter(s));
        }

        public static bool CompareUrlBySegments(this string baseString, string stringToCompare)
        {
            var firstLength = baseString.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length;
            var secondLength = stringToCompare.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length;

            return firstLength == secondLength;
        }

        public static bool IsDigitsOnly(this string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }
}
