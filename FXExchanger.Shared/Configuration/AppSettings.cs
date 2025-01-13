namespace FXExchanger.Shared.Configuration
{
    public record AppSettings
    {
        public string? ExchangeRatesApiUrl { get; init; }
        public string? ApiKey { get; init; }
        public int CacheDurationMinutes { get; init; } = 30;
    }
}
