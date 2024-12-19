using KNAB_Assessment.ServiceDefaults.CommonModels;

namespace KNAB_Assessment.ApiService.Services;

public interface ICryptoCurrencyExchangeRateService
{
    Task<GetExchangeRateResponse> GetExchangeRate(string symbol, string[] currencyExchanges);
}
