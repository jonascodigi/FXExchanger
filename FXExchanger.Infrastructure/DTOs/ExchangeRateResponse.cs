using Newtonsoft.Json;

namespace FXExchanger.Infrastructure.DTOs
{
    public record ExchangeRateResponse
    {
        [JsonProperty("data")]
        public Dictionary<string, decimal> Data = new();
    }
}
