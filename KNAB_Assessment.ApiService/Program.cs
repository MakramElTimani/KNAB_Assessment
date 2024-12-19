using KNAB_Assessment.ApiService.Repositories.CoinMarketCap;
using KNAB_Assessment.ApiService.Services;
using KNAB_Assessment.ApiService.Settings;
using KNAB_Assessment.ServiceDefaults.Exceptions;


var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// configure settings
builder.Services.Configure<CoinMarketCapSettings>(builder.Configuration.GetSection(nameof(CoinMarketCapSettings)));

// add problem details for better problem responses
builder.Services.AddProblemDetails();

// add custom exception handler
builder.Services.AddExceptionHandler<ProblemExceptionHandler>();

// setup http client for coinmarketcap
builder.Services.AddHttpClient(CryptoCurrencyRepository.HTTP_CLIENT_NAME, (client) =>
{
    var options = builder.Configuration.GetSection(nameof(CoinMarketCapSettings)).Get<CoinMarketCapSettings>();
    if (!string.IsNullOrEmpty(options?.Url) && !string.IsNullOrEmpty(options.ApiKey))
    {
        client.BaseAddress = new Uri(options.Url);
        client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", options.ApiKey);
    }
    else
    {
        // Handle the case when options are null
        // You can throw an exception, log an error, or provide a default value
        throw new InvalidOperationException("CoinMarketCapSettings options are null.");
    }
});

// Add services to the container.
builder.Services.AddScoped<ICryptoCurrencyRepository, CryptoCurrencyRepository>();
builder.Services.AddScoped<ICryptoCurrencyExchangeRateService, CryptoCurrencyExchangeRateService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// this service has one method to get the exchange rate of the crypto
app.MapGet("/api/exchange/{symbol}/{currencies}", async (string symbol, string currencies, ICryptoCurrencyExchangeRateService cryptoCyrrencyExchangeRateService) =>
{
    return await cryptoCyrrencyExchangeRateService.GetExchangeRate(symbol, currencies.Split(','));
});

app.MapDefaultEndpoints();

app.Run();

