namespace FXExchanger.Infrastructure.HttpClients
{
    public interface IFreeCurrencyApiClient
    {
        Task<Dictionary<string, decimal>> GetExchangeRatesAsync(string fromCurrency, string toCurrency);
    }
}
