using CryptoWatch.Integration.Binance.ApiRest.Interfaces;
using CryptoWatch.Repository.Interfaces;
using CryptoWatch.Services.Interfaces;
using Microsoft.VisualBasic;

namespace CryptoWatch.Services;

public class TickerPriceServices : ITickerPriceServices
{
    private readonly ISymbolRepository _symbolRepository;
    private readonly ITickerPriceIntegration _tickerPriceIntegration;

    public TickerPriceServices(ISymbolRepository symbolRepository, ITickerPriceIntegration tickerPriceIntegration)
    {
        _symbolRepository = symbolRepository;
        _tickerPriceIntegration = tickerPriceIntegration;
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
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{DateTime.UtcNow:o} => Symbol: {tickerPrice.Symbol}, Price: {tickerPrice.LastPrice}, Volume: {tickerPrice.Volume}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}