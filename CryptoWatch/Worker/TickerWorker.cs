using CryptoWatch.Integration.Binance.ApiRest.Interfaces;

namespace CryptoWatch.Worker;

public class TickerWorker : BackgroundService
{
    private readonly ILogger<TickerWorker> _logger;
    private readonly ITickerPriceIntegration _tickerPriceIntegration;

    public TickerWorker(ILogger<TickerWorker> logger, ITickerPriceIntegration tickerPriceIntegration)
    {
        _logger = logger;
        _tickerPriceIntegration = tickerPriceIntegration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {  
           
            
            await Task.Delay(5000, stoppingToken);
        }
    }
}