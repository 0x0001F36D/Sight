// Author: Viyrex(aka Yuyu)
// Contact: mailto:viyrex.aka.yuyu@gmail.com
// Github: https://github.com/0x0001F36D

namespace Sight.Console
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Nancy;

    public class Restful : NancyModule
    {

        static Data s_cache = default;

        #region Constructors

        public Restful()
        {
            Get["/interval={i}"] = Post["/interval={i}"] = (arg) =>
            {
                var interval = 300;
                if (arg.i > 0)
                    interval = arg.i;

                using (var sw = new StreamReader(MainClass.Opt.LogPath))
                {
                    var page = sw.ReadToEnd();
                    var lines = page.Split(new[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    return Response.AsText(this.BuildHtml(null, bodySegment: $"<p>Request time: [{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss.ffff")}]<br>Last updated: {lines.Last()}</p>", autoRefreshTimeSeconds: interval)).WithContentType("text/html");
                }
            };
            Get["/"] = Post["/"] = (_) => Response.AsRedirect("/interval=300");


            Get["/q"] = (_) => Response.AsRedirect("/q/from=0");
            Get["/q/from={from}"] = (arg) =>
            {
                if (arg.from < 0)
                    return Response.AsText("Error").WithStatusCode(HttpStatusCode.BadRequest);

                var sb = new StringBuilder();
                using (var sw = new StreamReader(MainClass.Opt.LogPath))
                {
                    var page = sw.ReadToEnd();
                    var lines = page.Split(new[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    if (lines.Length == 0)
                        return Response.AsText("No content");
                    if (lines.Length < arg.from)
                        return Response.AsText("Error").WithStatusCode(HttpStatusCode.BadRequest);

                    for (int i = arg.from; i < lines.Length; i++)
                    {
                        sb.AppendFormat("{0}<br>", lines[i]);
                    }

                    var html = this.BuildHtml("Log", bodySegment: sb.ToString(), autoRefreshTimeSeconds:9600);

                    return Response.AsText(html).WithContentType("text/html"); ;
                }
            };
            Get["/q/from={from}&count={count}"] = (arg) =>
            {
                if (arg.from > arg.count || arg.from < 0 || arg.count < 0)
                    return Response.AsText("Error").WithStatusCode(HttpStatusCode.BadRequest);

                var sb = new StringBuilder();
                using (var sw = new StreamReader(MainClass.Opt.LogPath))
                {
                    var page = sw.ReadToEnd();

                    var lines = page.Split(new[] { "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    var range = Enumerable.Range(arg.from, arg.count);

                    if (lines.Length < arg.from || (arg.from - arg.count) > lines.Length || arg.count > lines.Length)
                        return Response.AsText("Error").WithStatusCode(HttpStatusCode.BadRequest);

                    foreach (var index in range)
                    {
                        sb.AppendFormat("{0}<br>", lines[index]);
                    }

                    var html = this.BuildHtml("Log", bodySegment: sb.ToString(), autoRefreshTimeSeconds: 9600);

                    return Response.AsText(html).WithContentType("text/html");
                }
            };

            Get["/data/p={p}&t={t}&h={h}"] = (arg) =>
            {
                var password = (string)arg.p;
                if (password.WithHash() != MainClass.Opt.Password)
                {
                    return Response.AsText("Error").WithStatusCode(HttpStatusCode.Forbidden);
                }
                var d = new Data
                {
                    Time = DateTime.Now,
                    Humidity = arg.h,
                    Temperature = arg.t
                };
                if (s_cache is null || !s_cache.Equals(d))
                { 
                    s_cache = d; 
                    File.AppendAllLines(MainClass.Opt.LogPath, new string[1] { d.ToString() });
                    Console.Title = ($"Temperature: {d.Temperature}, Humidity: {d.Humidity}");

                }
                return Response.AsJson(d);
            };
        }

        #endregion Constructors

        #region Fields

        private string HTML_TEMPLETE = @"
<!DOCTYPE html>
    <html>
        <head>
            <title>{0}</title>
            <script type=""text/JavaScript"">
                function AutoRefresh(t) {{
                    if(t === -1)
                        return;
                    setTimeout(""location.reload(true);"", t);
                }}
            </script>
            {1}
        <head>
        <body onload=""JavaScript: AutoRefresh({2});"" \>
            {3}
        </body>
    </html>
";

        #endregion Fields

        #region Methods

        private string BuildHtml(string title = "Sight", string headSegment = null, string bodySegment = null, int autoRefreshTimeSeconds = -1)
        {
            return string.Format(HTML_TEMPLETE, title, headSegment, autoRefreshTimeSeconds * 1000, bodySegment);
        }

        #endregion Methods
    }
}