// Author: Viyrex(aka Yuyu)
// Contact: mailto:viyrex.aka.yuyu@gmail.com
// Github: https://github.com/0x0001F36D

namespace Sight.Console
{
    using System;
    using System.IO;

    using CommandLine;

    using Nancy;
    using Nancy.Hosting.Self;

    internal sealed class MainClass
    {
        #region Properties

        internal static Options Opt { get; private set; }

        #endregion Properties

        #region Methods

        private static void Main(string[] args)
        {
            try
            {
                var host = default(NancyHost);
                Parser.Default.ParseArguments<Options>(args).WithParsed(opt =>
                {
                    Opt = opt;

                    if (!File.Exists(opt.LogPath))
                        File.Create(opt.LogPath);

                    var uriString = "http://localhost:{0}";
                    var url = new Uri(string.Format(uriString, opt.Port));
                    StaticConfiguration.Caching.EnableRuntimeViewUpdates = true;
                    var hostConfig = new HostConfiguration
                    {
                        AllowChunkedEncoding = false,
                    };

                    host = new NancyHost(hostConfig, url);
                });
                if (host != null)
                    using (host)
                    {
                        host.Start();
                        Console.WriteLine("Start");
                        Console.WriteLine("Press any key to exit.");
                        Console.ReadKey();
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion Methods
    }
}