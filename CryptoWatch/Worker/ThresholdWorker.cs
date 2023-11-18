using System.Runtime.InteropServices.JavaScript;
using Binance.Spot;
using CryptoWatch.Integration.Binance.ApiRest.Domain;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace CryptoWatch.Worker;

public class ThresholdWorker : BackgroundService
{
    private readonly ILogger<ThresholdWorker> _logger;
    private readonly IDatabase _redisDatabaseLastPrices;

    public ThresholdWorker(ILogger<ThresholdWorker> logger, [FromKeyedServices("LastPrices")] IDatabase redisDatabaseLastPrices)
    {
        _logger = logger;
        _redisDatabaseLastPrices = redisDatabaseLastPrices;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var lastPrices = _redisDatabaseLastPrices.HashGetAll("LastPrices").
                    Select(s => new SymbolPrice() { Symbol=s.Name, LastPrice= s.Value});

            foreach (var symbolPrice in lastPrices)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{DateTime.Now:o} => Symbol: {symbolPrice.Symbol} / {symbolPrice.LastPrice}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            await Task.Delay(5000, stoppingToken);
        }
    }
}