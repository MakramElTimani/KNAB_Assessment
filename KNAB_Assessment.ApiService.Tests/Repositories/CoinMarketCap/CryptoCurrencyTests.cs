using FluentAssertions;
using KNAB_Assessment.ApiService.Repositories.CoinMarketCap;
using KNAB_Assessment.ApiService.Repositories.CoinMarketCap.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace KNAB_Assessment.Tests.Repositories.CoinMarketCap;

public class CryptoCurrencyTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly Mock<ILogger<CryptoCurrencyRepository>> _loggerMock;
    private readonly CryptoCurrencyRepository _cryptoCurrencyRepository;

    public CryptoCurrencyTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _loggerMock = new Mock<ILogger<CryptoCurrencyRepository>>();

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://testurl.com/")
        };

        _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        _cryptoCurrencyRepository = new CryptoCurrencyRepository(_httpClientFactoryMock.Object, _loggerMock.Object);
    }

    private void MockHttpResponse(HttpStatusCode statusCode, object? content)
    {
        var response = new HttpResponseMessage
        {
            StatusCode = statusCode,
        };

        if (statusCode == HttpStatusCode.OK && content is not null)
        {
            response.Content = new StringContent(JsonSerializer.Serialize(content));
        }

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
    }

    [Fact]
    public async Task Map_ShouldReturnValidResponse_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedResponse = new ListingResponse<CryptoCurrency>
        {
            Data = new List<CryptoCurrency> { new CryptoCurrency() }
        };

        MockHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _cryptoCurrencyRepository.Map("BTC");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Data.Count, result.Data.Count);
    }

    [Fact]
    public async Task Map_ShouldThrowException_WhenApiResponseIsInvalid()
    {
        // Arrange
        MockHttpResponse(HttpStatusCode.BadRequest, null);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => _cryptoCurrencyRepository.Map("Btss"));
    }

    [Fact]
    public async Task GetLatestQuotes_ShouldReturnValidResponse_WhenApiCallIsSuccessful()
    {
        // Arrange
        var quote = new Quote() { Price = 0.9M, LastUpdated = DateTime.Now };
        var quoteResponse = new QuoteResponse()
        {
            Id = 1,
            Name = "Bitcoin",
            Symbol = "BTC",
            Quote = new Dictionary<string, Quote>
            {
                { "UDS", quote },
            }
        };
        var expectedResponse = new ObjectResponse<Dictionary<string, QuoteResponse>>
        {
            Data = new Dictionary<string, QuoteResponse>
            {
                { "BTC", quoteResponse }
            }
        };

        MockHttpResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var result = await _cryptoCurrencyRepository.LatestQuotesV2(["BTC,ETH"]);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
        expectedResponse.Data.Count.Should().Be(result.Data!.Count);
    }

    [Fact]
    public async Task GetLatestQuotes_ShouldThrowException_WhenApiResponseIsInvalid()
    {
        // Arrange
        MockHttpResponse(HttpStatusCode.BadRequest, null);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => _cryptoCurrencyRepository.LatestQuotesV2(["BTC"]));
    }

}
