using System.Text.Json;
using CryptoWatch.Integration.Binance.ApiRest.Domain;
using StackExchange.Redis;

namespace CryptoWatch.Worker;

public class ThresholdSymbolNotificationWorker : BackgroundService
{
    private readonly ILogger<ThresholdWorker> _logger;
    private readonly ISubscriber _subscriber;


    public ThresholdSymbolNotificationWorker(
        ILogger<ThresholdWorker> logger,  
        [FromKeyedServices("Publisher")]ISubscriber subscriber)
    {
        _logger = logger;
        _subscriber = subscriber;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _subscriber.SubscribeAsync("SymbolPrice", (channel, message) =>
        {
           var lastPrice = JsonSerializer.Deserialize<SymbolPrice>(message);
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Received lastprice from observer: {channel} {lastPrice.Symbol}", channel, lastPrice);
            Console.ForegroundColor = ConsoleColor.White;
        });
    }
}