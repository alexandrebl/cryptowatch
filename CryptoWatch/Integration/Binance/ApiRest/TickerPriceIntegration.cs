using System.Text.Json;
using CryptoWatch.Integration.Binance.ApiRest.Domain;
using CryptoWatch.Integration.Binance.ApiRest.Interfaces;

namespace CryptoWatch.Integration.Binance.ApiRest;

public class TickerPriceIntegration : ITickerPriceIntegration
{
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly JsonSerializerOptions _serializeOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public TickerPriceIntegration(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IEnumerable<TickerPrice> GetPrices(IEnumerable<string> symbols)
    {
        var tickerPrices = new List<TickerPrice>();

        symbols.ToList().ForEach(symbol => tickerPrices.Add(GetPrice(symbol).Result));

        return tickerPrices;
    }

    private async Task<TickerPrice> GetPrice(string symbol)
    {
        var contentStream = await OnGet(symbol);

        if (contentStream == string.Empty)
            throw new Exception($"Was not possible to got ticker price for symbol: {symbol}");

        var tickerPrice = MapperFromStream(contentStream, symbol);

        return tickerPrice;
    }

    private TickerPrice MapperFromStream(string contentStream, string symbol)
    {
        var tickerPrice = JsonSerializer.Deserialize<TickerPrice?>(contentStream, _serializeOptions);

        if (tickerPrice is null)
            throw new Exception($"Was not possible to deserialize data to ticker price for symbol: {symbol}");

        return tickerPrice;
    }

    private async Task<string> OnGet(string symbol)
    {
        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://api.binance.com/api/v3/ticker?symbol={symbol}");

        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (!httpResponseMessage.IsSuccessStatusCode) return string.Empty;
        
        var contentStream =
            await httpResponseMessage.Content.ReadAsStringAsync();

        return contentStream;
    }
}