using System.Text.Json.Serialization;

namespace KNAB_Assessment.ApiService.Repositories.CoinMarketCap.Models;

public class ListingResponse<T>
{
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();

    [JsonPropertyName("status")]
    public ListingResponseStatus Status { get; set; } = new();
}

public class ObjectResponse<T>
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("status")]
    public ListingResponseStatus Status { get; set; } = new();
}

public class ListingResponseStatus
{
    [JsonPropertyName("timestamp")]
    public DateTime? Timestamp { get; set; }

    [JsonPropertyName("error_code")]
    public int ErrorCode { get; set; }

    [JsonPropertyName("error_message")]
    public string? ErrorMessage { get; set; }

    [JsonPropertyName("elapsed")]
    public int Elapsed { get; set; }

    [JsonPropertyName("credit_count")]
    public int CreditCount { get; set; }
}
