namespace FXExchanger.Core.Interfaces
{
    public interface IExchangeRateProvider
    {
        decimal GetRate(string fromCurrency, string toCurrency);
    }
}
