using FluentAssertions;
using KNAB_Assessment.ApiService.Repositories.CoinMarketCap;
using KNAB_Assessment.ApiService.Repositories.CoinMarketCap.Models;
using KNAB_Assessment.ApiService.Services;
using KNAB_Assessment.ServiceDefaults.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace KNAB_Assessment.ApiService.Tests.Services;
public class CryptoCurrencyExchangeRateServiceTests
{
    private readonly Mock<ICryptoCurrencyRepository> _cryptoCurrencyRepositoryMock;
    private readonly CryptoCurrencyExchangeRateService _service;
    private readonly Mock<ILogger<CryptoCurrencyExchangeRateService>> _loggerMock;

    public CryptoCurrencyExchangeRateServiceTests()
    {
        _cryptoCurrencyRepositoryMock = new Mock<ICryptoCurrencyRepository>();
        _loggerMock = new Mock<ILogger<CryptoCurrencyExchangeRateService>>();
        _service = new CryptoCurrencyExchangeRateService(_cryptoCurrencyRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetExchangeRate_ShouldReturnValidResponse_WhenApiCallIsSuccessful()
    {
        // Arrange
        string symbol = "BTC";
        string[] currencyExchanges = { "USD", "EUR" };
        var listingResponse = new ListingResponse<CryptoCurrency>
        {
            Data = new List<CryptoCurrency> { new CryptoCurrency { Id = 1, Symbol = "BTC" } }
        };
        var quoteResponse = new ObjectResponse<Dictionary<string, QuoteResponse>>
        {
            Data = new Dictionary<string, QuoteResponse>
                {
                    { "1", new QuoteResponse
                    {
                        Id = 1,
                        Symbol = "BTC",
                        Quote = new Dictionary<string, Quote>
                        {
                            { "USD", new Quote { Price = 50000 } },
                            { "EUR", new Quote { Price = 45000 } }
                        }
                    }
                }
                }
        };

        _cryptoCurrencyRepositoryMock.Setup(repo => repo.Map(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(listingResponse);
        _cryptoCurrencyRepositoryMock.Setup(repo => repo.LatestQuotesV2(It.IsAny<int[]>(), It.IsAny<string>())).ReturnsAsync(quoteResponse);

        // Act
        var result = await _service.GetExchangeRate(symbol, currencyExchanges);

        // Assert
        result.Should().NotBeNull();
        result.Symbol.Should().Be(symbol);
        result.ExchangeRates.Should().HaveCount(2);
        result.ExchangeRates.Should().ContainKey("USD");
        result.ExchangeRates.Should().ContainKey("EUR");
        result.ExchangeRates["USD"].Should().Be(50000);
        result.ExchangeRates["EUR"].Should().Be(45000);
    }

    [Fact]
    public async Task GetExchangeRate_ShouldThrowProblemException_WhenSymbolIsEmpty()
    {
        // Arrange
        string symbol = "";
        string[] currencyExchanges = ["USD"];

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ProblemException>(() => _service.GetExchangeRate(symbol, currencyExchanges));
        exception.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        exception.Error.Should().Be("Bad Request");
    }

    [Fact]
    public async Task GetExchangeRate_ShouldThrowProblemException_WhenCurrencyExchangesAreEmpty()
    {
        // Arrange
        string symbol = "BTC";
        string[] currencyExchanges = [];

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ProblemException>(() => _service.GetExchangeRate(symbol, currencyExchanges));
        exception.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        exception.Error.Should().Be("Bad Request");
    }

    [Fact]
    public async Task GetExchangeRate_ShouldThrowProblemException_WhenCryptoCurrencyNotFound()
    {
        // Arrange
        string symbol = "BTss";
        string[] currencyExchanges = ["USD"];

        _cryptoCurrencyRepositoryMock.Setup(repo => repo.Map(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ProblemException>(() => _service.GetExchangeRate(symbol, currencyExchanges));
        exception.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        exception.Error.Should().Be("Not Found");
    }

}
