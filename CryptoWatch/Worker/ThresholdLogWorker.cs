using CryptoWatch.Integration.Binance.ApiRest.Domain;
using EasyNetQ;

namespace CryptoWatch.Worker;

public class ThresholdLogWorker : BackgroundService
{
    private readonly ILogger<ThresholdWorker> _logger;
    private readonly IBus _bus;
    
    public ThresholdLogWorker(ILogger<ThresholdWorker> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<SymbolPrice>("Log", HandleSymbolPrice);
    }
    
    static void HandleSymbolPrice(SymbolPrice symbolPrice) 
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("Got message from bus to log: {0}", symbolPrice.Symbol);
        Console.ResetColor();
    }
}