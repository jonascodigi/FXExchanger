using FXExchanger.ConsoleApp.Application;
using FXExchanger.Core.Interfaces;
using FXExchanger.Core.Services;
using FXExchanger.Core.Strategies;
using FXExchanger.Infrastructure.HttpClients;
using FXExchanger.Infrastructure.Providers;
using FXExchanger.Shared.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FXExhanger
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            Configuration = BuildConfiguration();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();

            if (appSettings == null)
                throw new InvalidOperationException("AppSettings section not found in configuration.");

            if (string.IsNullOrWhiteSpace(appSettings.ExchangeRatesApiUrl))
                throw new InvalidOperationException("ExchangeRatesApiUrl is missing or empty in AppSettings.");

            services.AddSingleton(appSettings);

            services.AddHttpClient<IFreeCurrencyApiClient, FreeCurrencyApiClient>(client => 
            {
                client.BaseAddress = new Uri(appSettings.ExchangeRatesApiUrl);
            });

            services.AddMemoryCache();
            services.AddTransient<ApiExchangeRateProvider>();
            services.AddTransient<ICurrencyConversionStrategy, StandardConversionStrategy>();
            services.AddTransient<CurrencyConverter>();
            services.AddTransient<IExchangeRateProvider>(sp => {
                var innerProvider = sp.GetRequiredService<ApiExchangeRateProvider>();
                var cache = sp.GetRequiredService<IMemoryCache>();

                return new CachingExchangeProvider(innerProvider, cache, appSettings);
            });
            services.AddSingleton<ICurrencyValidator, IsoCurrencyValidator>();
            services.AddTransient<ConsoleHandler>();
        }

        private static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
    }

}
