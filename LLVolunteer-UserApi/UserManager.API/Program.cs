using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UserManager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    if (context.HostingEnvironment.IsProduction())
                    {
                        // apollo  输出在 控制台
                        Com.Ctrip.Framework.Apollo.Logging.LogManager.Provider = new ConsoleLoggerProvider(Com.Ctrip.Framework.Apollo.Logging.LogLevel.Trace);
                        config.AddApollo(config.Build().GetSection("apollo"))
                            .AddDefault();
                    }
                })
                .UseStartup<Startup>();
    }
}
