using TollCalculator.Configurations;
using TollCalculator.Configurations.TollCalculator.Configuration;
using TollCalculator.Services;

namespace TollCalculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            var tollCalculatorSettings = configuration.GetSection("TollCalculatorSettings").Get<TollCalculatorSettings>();

            var services = new ServiceCollection();
            services.Configure<TollCalculatorSettings>(configuration.GetSection("TollCalculatorSettings"));

            services.AddSingleton<TollCalculatorService>();

            var serviceProvider = services.BuildServiceProvider();

            var tollCalculator = serviceProvider.GetRequiredService<TollCalculatorService>();
            Console.WriteLine($"MaxDailyFee: {tollCalculatorSettings.MaxDailyFee}");

            app.MapGet("/", () => $"MaxDailyFee: {tollCalculatorSettings.MaxDailyFee}");

            app.Run();
        }
    }
}
