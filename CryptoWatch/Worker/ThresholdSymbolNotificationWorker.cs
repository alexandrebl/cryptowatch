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
        await _subscriber.SubscribeAsync("SymbolPrice",  (channel, message) =>
        {
           var lastPrice = JsonSerializer.Deserialize<SymbolPrice>(message);
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Received lastprice from observer: {channel} {lastPrice.Symbol}", channel, lastPrice);
            Console.ForegroundColor = ConsoleColor.White;

            PublishOnBus(lastPrice);
        });
    }

    private void PublishOnBus(SymbolPrice lastPrice)
    { 
        _bus.PubSub.Publish(lastPrice);
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Lastprice published on bus:  {lastPrice.Symbol}");
        Console.ForegroundColor = ConsoleColor.White;
    }
}