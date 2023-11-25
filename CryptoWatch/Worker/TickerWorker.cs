using CryptoWatch.Integration.Binance.ApiRest.Interfaces;
using CryptoWatch.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace CryptoWatch.Worker;

public class TickerWorker : BackgroundService
{
    private readonly ILogger<TickerWorker> _logger;
    private readonly ITickerPriceServices _tickerPriceServices;

    public TickerWorker(ILogger<TickerWorker> logger, ITickerPriceServices tickerPriceServices)
    {
        _logger = logger;
        _tickerPriceServices = tickerPriceServices;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _tickerPriceServices.UpdateTickerPrice();
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}