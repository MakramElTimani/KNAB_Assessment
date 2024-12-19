
namespace KNAB_Assessment.ServiceDefaults.CommonModels;
public class GetExchangeRateResponse
{
    public string? Symbol { get; set; }

    public string? Name { get; set; }
    public Dictionary<string, decimal> ExchangeRates { get; set; } = new();
}
