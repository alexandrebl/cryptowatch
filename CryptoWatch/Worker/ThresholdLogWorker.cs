using CryptoWatch.Integration.Binance.ApiRest.Domain;
using CryptoWatch.Repository.Interfaces;
using EasyNetQ;

namespace CryptoWatch.Worker;

public class ThresholdLogWorker : BackgroundService
{
    private readonly ILogger<ThresholdWorker> _logger;
    private readonly IBus _bus;
    private readonly IThresholdLogRepository _thresholdLogRepository;

    public ThresholdLogWorker(ILogger<ThresholdWorker> logger, IBus bus, IThresholdLogRepository thresholdLogRepository)
    {
        _logger = logger;
        _bus = bus;
        _thresholdLogRepository = thresholdLogRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<SymbolPrice>("Log", HandleSymbolPrice);
    }

    private void HandleSymbolPrice(SymbolPrice symbolPrice) 
    {
        _thresholdLogRepository.Register(symbolPrice);
        
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("Got message from bus to log: {0}", symbolPrice.Symbol);
        Console.ResetColor();
    }
}