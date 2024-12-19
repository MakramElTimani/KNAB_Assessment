using KNAB_Assessment.ApiService.Repositories.CoinMarketCap.Models;
using System.Text;

namespace KNAB_Assessment.ApiService.Repositories.CoinMarketCap;

public class CryptoCurrencyRepository : ICryptoCurrencyRepository
{
    private readonly HttpClient _httpClient;

    public const string HTTP_CLIENT_NAME = "CoinMarketCap";

    private readonly ILogger<CryptoCurrencyRepository> _logger;

    public CryptoCurrencyRepository(
        IHttpClientFactory httpClientFactory,
        ILogger<CryptoCurrencyRepository> logger)
    {
        _httpClient = httpClientFactory.CreateClient(HTTP_CLIENT_NAME);
        _logger = logger;
    }

    public async Task<ListingResponse<CryptoCurrency>> Map(string? symbol, int? start = 1, int? limit = 5000)
    {
        StringBuilder urlBuilder = new StringBuilder("/v1/cryptocurrency/map");
        if (!string.IsNullOrWhiteSpace(symbol))
        {
            urlBuilder.Append($"?symbol={symbol}");
        }
        if (start.HasValue)
        {
            urlBuilder.Append(string.IsNullOrWhiteSpace(symbol) ? $"?start={start}" : $"&start={start}");
        }
        if (limit.HasValue)
        {
            urlBuilder.Append(string.IsNullOrWhiteSpace(symbol) && !start.HasValue ? $"?limit={limit}" : $"&limit={limit}");
        }
        string url = urlBuilder.ToString();
        try
        {
            ListingResponse<CryptoCurrency>? response = await _httpClient.GetFromJsonAsync<ListingResponse<CryptoCurrency>>(url);
            if (response is null)
            {
                _logger.LogError("Failed to map response of 'map' to model; URL: {RequestUrl}", url);
                throw new InvalidOperationException("Failed to map response to model");
            }
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching data from CoinMarketCap; function: Map; Symbol: {Symbol}", symbol);
            throw new HttpRequestException($"Error while fetching data from CoinMarketCap: {ex.Message}");
        }
    }

    public Task<ObjectResponse<Dictionary<string, QuoteResponse>>> LatestQuotesV2(string[] symbols, string? convert = null)
    {
        StringBuilder urlBuilder = new("/v2/cryptocurrency/quotes/latest");
        string symbolsString = string.Join(",", symbols);
        urlBuilder.Append($"?symbol={symbolsString}");
        if (!string.IsNullOrWhiteSpace(convert))
        {
            urlBuilder.Append($"&convert={convert}");
        }
        string url = urlBuilder.ToString();
        return LatestQuotesV2(url);
    }

    public Task<ObjectResponse<Dictionary<string, QuoteResponse>>> LatestQuotesV2(int[] ids, string? convert = null)
    {
        StringBuilder urlBuilder = new("/v2/cryptocurrency/quotes/latest");
        string idsString = string.Join(",", ids);
        urlBuilder.Append($"?id={idsString}");
        if (!string.IsNullOrWhiteSpace(convert))
        {
            urlBuilder.Append($"&convert={convert}");
        }
        string url = urlBuilder.ToString();
        return LatestQuotesV2(url);
    }

    private async Task<ObjectResponse<Dictionary<string, QuoteResponse>>> LatestQuotesV2(string url)
    {

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url)!;
            ObjectResponse<Dictionary<string, QuoteResponse>>? responseObject = await response.Content.ReadFromJsonAsync<ObjectResponse<Dictionary<string, QuoteResponse>>>();

            if (responseObject is null)
            {
                _logger.LogError("Failed to map response of latest quotes to model; URL: {RequestUrl}", url);
                throw new InvalidOperationException("Failed to map response to model");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(responseObject.Status.ErrorMessage ?? "Failed to get latest quote");
            }

            return responseObject;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching data from CoinMarketCap; URL: {RequestUrl}", url);
            throw new HttpRequestException(ex.Message);
        }
    }
}
