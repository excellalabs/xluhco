using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace xluhco.web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var startedApp = await Host.CreateDefaultBuilder(args)
                .UseSerilog().StartAsync();

            await startedApp.WaitForShutdownAsync();
        } 
    }
}
