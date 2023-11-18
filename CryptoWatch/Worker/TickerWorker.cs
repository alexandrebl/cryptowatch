using CryptoWatch.Integration.Binance.ApiRest.Interfaces;
using CryptoWatch.Services.Interfaces;

namespace CryptoWatch.Worker;

public class TickerWorker : BackgroundService
{
    private readonly ILogger<TickerWorker> _logger;
    private readonly ISymbolWatchServices _symbolWatchServices;

    public TickerWorker(ILogger<TickerWorker> logger, ISymbolWatchServices symbolWatchServices)
    {
        _logger = logger;
        _symbolWatchServices = symbolWatchServices;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _symbolWatchServices.UpdateTickerPrice();
            
            await Task.Delay(5000, stoppingToken);
        }
    }
}