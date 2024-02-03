using CryptoWatch.Integration.Binance.ApiRest.Domain;
using EasyNetQ;
using StackExchange.Redis;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CryptoWatch.Worker;

public class ThresholdSymbolNotificationWorker : BackgroundService
{
    private readonly ILogger<ThresholdWorker> _logger;
    private readonly ISubscriber _subscriber;
    private readonly IBus _bus;

    public ThresholdSymbolNotificationWorker(
        ILogger<ThresholdWorker> logger,  
        [FromKeyedServices("Publisher")]ISubscriber subscriber, IBus bus)
    {
        _logger = logger;
        _subscriber = subscriber;
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _subscriber.SubscribeAsync(nameof(SymbolPriceUpOrDownResult),  (channel, message) =>
        {
           var symbolPriceUpOrDownResult = JsonSerializer.Deserialize<SymbolPriceUpOrDownResult>(message);
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Received lastprice from observer: {channel} {symbolPriceUpOrDownResult.LastPrice.Symbol}");
            Console.ForegroundColor = ConsoleColor.White;

            PublishOnBus(symbolPriceUpOrDownResult);
        });
    }

    private void PublishOnBus(SymbolPriceUpOrDownResult symbolPriceUpOrDownResult)
    { 
        _bus.PubSub.Publish(symbolPriceUpOrDownResult);
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Lastprice published on bus:  {symbolPriceUpOrDownResult.LastPrice.Symbol}");
        Console.ForegroundColor = ConsoleColor.White;
    }
}