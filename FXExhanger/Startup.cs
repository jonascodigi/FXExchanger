using FXExchanger.Core.Interfaces;
using FXExchanger.Core.Services;
using FXExchanger.Core.Strategies;
using FXExchanger.Infrastructure.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace FXExhanger
{
    internal class Startup
    {
        public IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddTransient<IExchangeRateProvider, MockExchangeRateProvider>();
            services.AddTransient<ICurrencyConversionStrategy, StandardConversionStrategy>();
            services.AddTransient<CurrencyConverter>();

            return services.BuildServiceProvider();
        }
    }
}
