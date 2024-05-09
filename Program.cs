using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using TollCalculator.Configurations;
using TollCalculator.Configurations.TollCalculator.Configuration;
using TollCalculator.Model;
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
                        services.AddHttpClient();
                        // Register services
                        services.Configure<TollCalculatorSettings>(hostContext.Configuration.GetSection("TollCalculatorSettings"));
                        services.AddScoped<TollCalculatorService>();
                    });
                    webBuilder.Configure((app) =>
                    {
                        app.UseRouting();

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapGet("/", async context =>
                            {
                                Console.WriteLine("test");
                                var tollCalculator = context.RequestServices.GetRequiredService<TollCalculatorService>();

                                // http://localhost:7232/?date=2024-05-07&vehicleType=Car
                                // Extract date and vehicle details from the request (assuming they are provided as query parameters)
                                var dateStr = context.Request.Query["date"];
                                var vehicleType = context.Request.Query["vehicleType"];
                                Console.WriteLine("Date" + dateStr);
                                Console.WriteLine("vehicleType" + vehicleType);

                                // Parse date string to DateTime
                                DateTime date;
                                if (!DateTime.TryParse(dateStr, out date))
                                {
                                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                    await context.Response.WriteAsync("Invalid date format. Please provide date in valid format.");
                                    return;
                                }

                                // Create Vehicle object with the provided type

                                // Calculate toll fee
                                int tollFee = tollCalculator.GetTollFee(new List<DateTime> { date }, vehicleType);
                                Console.WriteLine("Toll Fee: " + tollFee);

                                // Return the toll fee in the response
                                await context.Response.WriteAsync($"Toll fee for {date} with vehicle type {vehicleType}: {tollFee}");
                            });
                        });
                    });
                });
    }
}
