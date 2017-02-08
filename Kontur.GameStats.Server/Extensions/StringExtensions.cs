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
        public static string GetServiceName(this string url)
        {
            return url.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .FirstOrDefault();
        }

        public static string Exclude(this string baseString, string excludeString)
        {
            return baseString.Replace(excludeString, String.Empty);
        }
    }
}
