using System.Text.Json.Serialization;

namespace KNAB_Assessment.ApiService.Repositories.CoinMarketCap.Models;

public class QuoteResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("is_active")]
    public int IsActive { get; set; }

    [JsonPropertyName("quote")]
    public Dictionary<string, Quote> Quote { get; set; } = new();
}
