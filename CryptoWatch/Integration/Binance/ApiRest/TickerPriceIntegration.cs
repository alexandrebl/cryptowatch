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

        if (contentStream == Stream.Null)
            throw new Exception($"Was not possible to got ticker price for symbol: {symbol}");

        var tickerPrice = MapperFromStream(contentStream, symbol);

        return tickerPrice;
    }

    private TickerPrice MapperFromStream(Stream contentStream, string symbol)
    {
        var tickerPrice = JsonSerializer.Deserialize<TickerPrice?>(contentStream, _serializeOptions);

        if (tickerPrice is null)
            throw new Exception($"Was not possible to deserialize data to ticker price for symbol: {symbol}");

        return tickerPrice;
    }

    private async Task<Stream> OnGet(string symbol)
    {
        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://api.binance.com/api/v3/ticker?symbol={symbol}");

        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (!httpResponseMessage.IsSuccessStatusCode) return Stream.Null;

        await using var contentStream =
            await httpResponseMessage.Content.ReadAsStreamAsync();

        return contentStream;
    }
}