using CryptoWatch.Integration.Binance.ApiRest.Domain;
using CryptoWatch.Services.Interfaces;
using CryptoWatch.Worker;
using StackExchange.Redis;

namespace CryptoWatch.Services
{
    public class ThresholdServices : IThresholdServices
    {
        private readonly ILogger<ThresholdWorker> _logger;
        private readonly IDatabase _redisDatabaseLastPrices;

        public ThresholdServices(ILogger<ThresholdWorker> logger, [FromKeyedServices("LastPrices")] IDatabase redisDatabaseLastPrices)
        {
            _logger = logger;
            _redisDatabaseLastPrices = redisDatabaseLastPrices;
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var lastPrices = _redisDatabaseLastPrices.HashGetAll("LastPrices").
                                Select(s => new SymbolPrice() { Symbol = s.Name, LastPrice = s.Value });

            foreach (var symbolPrice in lastPrices)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{DateTime.Now:o} => Symbol: {symbolPrice.Symbol} / {symbolPrice.LastPrice}");
                Console.ForegroundColor = ConsoleColor.White;
            }

            await Task.CompletedTask;
        }
    }
}