using System.Text.Json.Serialization;

namespace KNAB_Assessment.ApiService.Repositories.CoinMarketCap.Models;
public class CryptoCurrency
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("rank")]
    public int Rank { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("is_active")]
    public int IsActive { get; set; }
}
