using System.Runtime.InteropServices.JavaScript;
using Binance.Spot;

namespace CryptoWatch.Worker;

public class InfoWorker : BackgroundService
{
    private readonly ILogger<InfoWorker> _logger;
    private readonly Market _market;
    
    public InfoWorker(ILogger<InfoWorker> logger)
    {
        _logger = logger;
        _market = new Market();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {  
            var serverTime = await _market.CheckServerTime();
            Console.WriteLine(serverTime);
            
            _logger.LogInformation("Worker running at: {time} / Binance server time: {serverTime}", DateTimeOffset.Now, serverTime);
            
            await Task.Delay(5000, stoppingToken);
        }
    }
}