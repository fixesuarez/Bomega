using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bomega.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration( ( hostBuilderContext, confBuilder ) =>
                {
                    confBuilder.AddJsonFile( "appSettings.json", optional: false, reloadOnChange: true );
                } )
                .ConfigureLogging( loggingBuilder =>
                {
                    loggingBuilder.AddConsole();
                    loggingBuilder.SetMinimumLevel( LogLevel.Information );
                } )
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();
    }
}
