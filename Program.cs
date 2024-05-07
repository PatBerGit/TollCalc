using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TollCalculator.Configurations;
using TollCalculator.Configurations.TollCalculator.Configuration;
using TollCalculator.Services;

namespace TollCalculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((hostContext, services) =>
                    {
                        // Register services
                        services.Configure<TollCalculatorSettings>(hostContext.Configuration.GetSection("TollCalculatorSettings"));
                        services.AddSingleton<TollCalculatorService>();
                    });
                    webBuilder.Configure((app) =>
                    {
                        app.UseRouting();

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapGet("/", async context =>
                            {
                                var tollCalculator = context.RequestServices.GetRequiredService<TollCalculatorService>();
                                var tollCalculatorSettings = context.RequestServices.GetRequiredService<IOptions<TollCalculatorSettings>>().Value;

                                // Access and use configured settings
                                await context.Response.WriteAsync($"MaxDailyFee: {tollCalculatorSettings.MaxDailyFee}");
                            });
                        });
                    });
                });
    }
}
