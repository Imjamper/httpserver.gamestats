using System;
using System.Linq;
using Fclp;
using GL.HttpServer;

namespace Kontur.GameStats.Server
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {
            var helpCalled = args.Contains("help") || args.Contains("h");

            var commandLineParser = new FluentCommandLineParser<Configuration>();

            commandLineParser
                .Setup(options => options.Prefix)
                .As("prefix")
                .SetDefault("http://+:8080/")
                .WithDescription("[--prefix <prefix>] HTTP prefix to listen on");

            commandLineParser
                .Setup(options => options.EnableLogging)
                .As("enableLogs")
                .SetDefault(true)
                .WithDescription("[--enableLogs <true/false>] Enable logs in console");

            commandLineParser
                .SetupHelp("h", "help")
                .WithHeader("The available server arguments:")
                .Callback(text =>
                {
                    Console.WriteLine(text);
                });
            commandLineParser.Parse(args);
            if (helpCalled)
            {
                commandLineParser.HelpOption.ShowHelp(commandLineParser.Options);
                Console.ReadLine();
                return;
            }

            new ServerStarter(commandLineParser.Object).Start();
        }
    }
}