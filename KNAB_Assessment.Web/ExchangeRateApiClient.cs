using KNAB_Assessment.ServiceDefaults.CommonModels;
using Microsoft.AspNetCore.Mvc;

namespace KNAB_Assessment.Web;

public class ExchangeRateApiClient(HttpClient httpClient)
{
    public async Task<GetExchangeRateResponse?> GetExchangeRate(string symbol, string currencies)
    {
        var response = await httpClient.GetAsync($"/api/exchange/{symbol}/{currencies}");
        if (!response.IsSuccessStatusCode)
        {
            ProblemDetails? problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            if (problemDetails is not null)
            {
                throw new HttpRequestException(problemDetails.Detail);
            }
            throw new HttpRequestException("Failed to retrieve data");
        }
        return await response.Content.ReadFromJsonAsync<GetExchangeRateResponse>();
    }
}
