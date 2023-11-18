using System.Runtime.InteropServices.JavaScript;
using Binance.Spot;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace CryptoWatch.Worker;

public class InfoWorker : BackgroundService
{
    private readonly ILogger<InfoWorker> _logger;
    private readonly IConnectionMultiplexer _redis;

    public InfoWorker(ILogger<InfoWorker> logger, IConnectionMultiplexer redis)
    {
        _logger = logger;
        _redis = redis;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // todo O indice do banco deveria ser variavel de ambiente
            var database = _redis.GetDatabase(0); 
            Console.WriteLine("Done");
            await Task.Delay(5000, stoppingToken);
        }
    }
}