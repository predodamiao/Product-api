using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;
using Serilog;
using Microsoft.Extensions.Logging;

namespace Infrastructure.IoC
{
    public static class InfrastructureIoC
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }

        public static IServiceCollection AddSerilogLoggin(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            services.AddSingleton(Log.Logger);

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            });

            return services;
        }
    }
}
