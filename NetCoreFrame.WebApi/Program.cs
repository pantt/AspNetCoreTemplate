using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using iCTR.TB.Framework.Core.Extensions;

namespace NetCoreFrame
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">参数集合</param>
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            host.Run();
        }
        /// <summary>
        /// BuildWebHost
        /// </summary>
        /// <param name="args">参数集合</param>
        /// <returns>IWebHost</returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .ConfigureServices(services => services.AddAutofac()) //autofac服务注册
                .UseStartup<Startup>()
                .Build();
    }
}