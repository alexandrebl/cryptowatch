using CryptoWatch.Integration.Binance.ApiRest.Interfaces;
using CryptoWatch.Repository.Interfaces;
using CryptoWatch.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.VisualBasic;

namespace CryptoWatch.Services;

public class TickerPriceServices : ITickerPriceServices
{
    private readonly ISymbolRepository _symbolRepository;
    private readonly ITickerPriceIntegration _tickerPriceIntegration;
    private readonly IDistributedCache _distributedCache;

    public TickerPriceServices(ISymbolRepository symbolRepository, 
        ITickerPriceIntegration tickerPriceIntegration, IDistributedCache distributedCache)
    {
        _symbolRepository = symbolRepository;
        _tickerPriceIntegration = tickerPriceIntegration;
        _distributedCache = distributedCache;
    }

    public async Task UpdateTickerPrice()
    {
        var symbolWatchItems = await _symbolRepository.GetSymbolsToWatchASync();

        if (!symbolWatchItems.Any())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("There is no symbol to watch");
            Console.ForegroundColor = ConsoleColor.White;
        }

        var symbols = symbolWatchItems.Select(s => s.Symbol);
        
        var tickerPrices = _tickerPriceIntegration.GetPrices(symbols);
        
        foreach (var tickerPrice in tickerPrices)
        {
            await SetPriceOnCache(tickerPrice.Symbol, tickerPrice.LastPrice);
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{DateTime.UtcNow:o} => Symbol: {tickerPrice.Symbol}, Price: {tickerPrice.LastPrice}, Volume: {tickerPrice.Volume}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    /// <summary>
    /// Grava último preço no cache
    /// </summary>
    /// <param name="symbol">Moeda</param>
    /// <param name="lastPrice">Último preço</param>
    /// <param name="expiration">Tempo para expirar. Cada loop 5 sec => 2x Loop + Metade do Loop</param>
    private async Task SetPriceOnCache(string symbol, string lastPrice, double expiration = 12)
    {
        await _distributedCache.SetStringAsync(symbol, lastPrice, new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expiration)
        });
        
    }
}