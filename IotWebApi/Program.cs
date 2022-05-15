using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace IotWebApi
{
    /// <summary>
    /// Main class who launch the WebApi
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The create the host builder
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }


        /// <summary>
        /// method that creates the host builder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
				});
    }
}
