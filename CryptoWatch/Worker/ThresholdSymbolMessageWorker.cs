using CryptoWatch.Integration.Binance.ApiRest.Domain;
using EasyNetQ;

namespace CryptoWatch.Worker;

public class ThresholdSymbolMessageWorker : BackgroundService
{
    private readonly ILogger<ThresholdWorker> _logger;
    private readonly IBus _bus;
    
    public ThresholdSymbolMessageWorker(ILogger<ThresholdWorker> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<SymbolPriceUpOrDownResult>("Message", HandleSymbolPrice);
    }
    
    static void HandleSymbolPrice(SymbolPriceUpOrDownResult symbolPriceUpOrDownResult) 
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Got message from bus to message: {0}", symbolPriceUpOrDownResult.LastPrice.Symbol);
        Console.ResetColor();
    }
}