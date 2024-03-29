using CryptoWatch.Integration.Binance.ApiRest.Domain;
using CryptoWatch.Integration.Binance.ApiRest.Interfaces;
using CryptoWatch.Repository.Interfaces;
using CryptoWatch.Services.Interfaces;
using StackExchange.Redis;

namespace CryptoWatch.Services;

public class TickerPriceServices : ITickerPriceServices
{
    private readonly ISymbolRepository _symbolRepository;
    private readonly ITickerPriceIntegration _tickerPriceIntegration;
    private readonly IDatabase _redisDatabaseLastPrices;

    public TickerPriceServices(ISymbolRepository symbolRepository, ITickerPriceIntegration tickerPriceIntegration,
        [FromKeyedServices("LastPrices")] IDatabase redisDatabaseLastPrices)
    {
        _symbolRepository = symbolRepository;
        _tickerPriceIntegration = tickerPriceIntegration;
        _redisDatabaseLastPrices = redisDatabaseLastPrices;
    }

    public async Task UpdateTickerPrice()
    {
        var symbolWatchItems = await _symbolRepository.GetSymbolsToWatchASync();

        if (!symbolWatchItems.Any())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("There is no symbol to watch");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }

        var symbols = symbolWatchItems.Select(s => s.Symbol);
        
        var tickerPrices = _tickerPriceIntegration.GetPrices(symbols);
        
        await SetPriceOnCache(tickerPrices);

        foreach (var tickerPrice in tickerPrices)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{DateTime.UtcNow:o} => Symbol: {tickerPrice.Symbol}, Price: {tickerPrice.LastPrice}, Volume: {tickerPrice.Volume}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    
    private async Task SetPriceOnCache(IEnumerable<TickerPrice> tickerPrices)
    {
        var hashEntries = tickerPrices.Select(s =>
            new HashEntry(s.Symbol, s.LastPrice)).ToArray();

        await _redisDatabaseLastPrices.HashSetAsync("LastPrices", hashEntries);
    }
}