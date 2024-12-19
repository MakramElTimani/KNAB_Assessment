
using KNAB_Assessment.ApiService.Repositories.CoinMarketCap;
using KNAB_Assessment.ApiService.Repositories.CoinMarketCap.Models;
using KNAB_Assessment.ServiceDefaults.CommonModels;
using KNAB_Assessment.ServiceDefaults.Exceptions;
using System.Net;

namespace KNAB_Assessment.ApiService.Services;

public class CryptoCurrencyExchangeRateService : ICryptoCurrencyExchangeRateService
{
    private readonly ICryptoCurrencyRepository _cryptoCurrencyRepository;
    private readonly ILogger<CryptoCurrencyExchangeRateService> _logger;

    public CryptoCurrencyExchangeRateService(
        ICryptoCurrencyRepository cryptoCurrencyRepository,
        ILogger<CryptoCurrencyExchangeRateService> logger)
    {
        _cryptoCurrencyRepository = cryptoCurrencyRepository;
        _logger = logger;
    }

    public async Task<GetExchangeRateResponse> GetExchangeRate(string symbol, string[] currencyExchanges)
    {
        // simple validations
        if (string.IsNullOrWhiteSpace(symbol))
        {
            throw new ProblemException(HttpStatusCode.BadRequest, "Bad Request", "Symbol cannot be empty");
        }
        // remove duplicates and empty strings and any commas
        // alternatively commas in the request could be flagged as bad request
        currencyExchanges = currencyExchanges
            .Distinct()
            .SelectMany(m => m.Trim().ToUpperInvariant().Split(','))
            .Where(m => !string.IsNullOrWhiteSpace(m))
            .ToArray();
        if (currencyExchanges.Length == 0)
        {
            throw new ProblemException(HttpStatusCode.BadRequest, "Bad Request", "Currency exchanges cannot be empty");
        }

        // try to find crypto currency
        CryptoCurrency? crypto = null;
        symbol = symbol.Trim().ToUpperInvariant();
        try
        {
            ListingResponse<CryptoCurrency> response = await _cryptoCurrencyRepository.Map(symbol);

            // .First guarantees that the symbol is found, if not found an exception is thrown
            crypto = response.Data.First(m => m.Symbol == symbol);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An attempt to find a crypto currency failed; symbol: {Symbol}", symbol);
            throw new ProblemException(HttpStatusCode.NotFound, "Not Found", $"Crypto currency {symbol} was not found");
        }

        try
        {
            GetExchangeRateResponse getExchangeRateResponse = new()
            {
                Symbol = crypto.Symbol,
                Name = crypto.Name,
                ExchangeRates = new Dictionary<string, decimal>()
            };

            // NOTE: because the free api version does not allow you to have multiple currencies in one call
            // I opted into a for loop to get the exchange rate for each currency
            // this might have some issues like inconsistencies between currencies
            // for example, if the coin price increases during the for loop, the currencies already looped will be cheaper than the ones still to be processed
            foreach (string exchange in currencyExchanges)
            {
                string currency = exchange.Trim().ToUpperInvariant();
                // get the latest quote
                ObjectResponse<Dictionary<string, QuoteResponse>> quoteResponse = await _cryptoCurrencyRepository.LatestQuotesV2([crypto.Id], currency);

                var price = quoteResponse.Data![crypto.Id.ToString()].Quote[currency].Price;
                getExchangeRateResponse.ExchangeRates.Add(currency, price);
            }
            return getExchangeRateResponse;
        }
        catch (Exception ex)
        {
            throw new ProblemException(HttpStatusCode.BadRequest, "Bad Request", ex.Message);
        }

    }
}
