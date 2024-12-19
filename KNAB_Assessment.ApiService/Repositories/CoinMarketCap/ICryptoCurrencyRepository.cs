using KNAB_Assessment.ApiService.Repositories.CoinMarketCap.Models;

namespace KNAB_Assessment.ApiService.Repositories.CoinMarketCap;

public interface ICryptoCurrencyRepository
{
    /// <summary>
    /// List the list of cryptocurrencies with search functionality by symbol and pagination.
    /// </summary>
    /// <param name="symbol">symbol to search for</param>
    /// <param name="start">Default 1; used for pagination</param>
    /// <param name="limit">Default 5000; used for pagination</param>
    /// <returns></returns>
    Task<ListingResponse<CryptoCurrency>> Map(string? symbol, int? start = 1, int? limit = 5000);

    /// <summary>
    /// Get the latest quotes for multiple cryptocurrencies.
    /// </summary>
    /// <param name="symbols">array of symbols to retrieve the latest quote for</param>
    /// <returns></returns>
    Task<ObjectResponse<Dictionary<string, QuoteResponse>>> LatestQuotesV2(string[] symbols, string? convertSymbol = null);

    /// <summary>
    /// Get the latest quotes for multiple cryptocurrencies.
    /// </summary>
    /// <param name="ids">array of crypt ids (can be retrieved from <see cref="Map(string?, int?, int?)"/>) </param>
    /// <returns></returns>
    Task<ObjectResponse<Dictionary<string, QuoteResponse>>> LatestQuotesV2(int[] ids, string? convertSymbol = null);
}
