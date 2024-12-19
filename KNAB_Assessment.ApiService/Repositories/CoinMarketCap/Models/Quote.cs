using System.Text.Json.Serialization;

namespace KNAB_Assessment.ApiService.Repositories.CoinMarketCap.Models;

public class Quote
{
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("percent_change_1h")]
    public decimal PercentChange1h { get; set; }

    [JsonPropertyName("percent_change_24h")]
    public decimal PercentChange24h { get; set; }

    [JsonPropertyName("last_updated")]
    public DateTime LastUpdated { get; set; }
}
