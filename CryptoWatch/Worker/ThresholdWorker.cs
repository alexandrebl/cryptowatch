using System.Runtime.InteropServices.JavaScript;
using Binance.Spot;
using CryptoWatch.Integration.Binance.ApiRest.Domain;
using CryptoWatch.Services.Interfaces;

namespace CryptoWatch.Worker;

public class ThresholdWorker : BackgroundService
{
    private readonly ILogger<ThresholdWorker> _logger;
    private readonly IThresholdServices _thresholdServices;

    public ThresholdWorker(ILogger<ThresholdWorker> logger, IThresholdServices thresholdServices)
    {
        _logger = logger;
        _thresholdServices = thresholdServices;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _thresholdServices.ExecuteAsync(stoppingToken);

            await Task.Delay(3000, stoppingToken);
        }
    }
}