using CryptoWatch.Integration.Binance.ApiRest.Domain;
using CryptoWatch.Services.Interfaces;
using CryptoWatch.Worker;
using StackExchange.Redis;
using System.Collections.Generic;
using CryptoWatch.Integration.Binance.ApiRest.Domain.Enums;

namespace CryptoWatch.Services
{
    public class ThresholdServices : IThresholdServices
    {
        private readonly ILogger<ThresholdWorker> _logger;
        private readonly IDatabase _redisDatabaseLastPrices;
        private IList<SymbolPrice> _currentPrices;
        private readonly ISubscriber _publisher;

        public ThresholdServices(ILogger<ThresholdWorker> logger,
            [FromKeyedServices("LastPrices")] IDatabase redisDatabaseLastPrices,
            [FromKeyedServices("Publisher")]ISubscriber publisher)
        {
            _logger = logger;
            _redisDatabaseLastPrices = redisDatabaseLastPrices;
            _currentPrices = new List<SymbolPrice>();
            _publisher = publisher;
        }
        
        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var lastPrices = _redisDatabaseLastPrices.HashGetAll("LastPrices").
                                Select(s => new SymbolPrice() { Symbol = s.Name, LastPrice = s.Value });

            foreach (var lastPrice in lastPrices)
            { 
                await GetUpDown(lastPrice);
            }
            
            await Task.CompletedTask;
        }

        public async Task GetUpDown(SymbolPrice lastPrice)
        {
            var threshold = 0.0001M; // TOdo varial de ambiente
            var currentPrice = _currentPrices.FirstOrDefault(f => f.Symbol == lastPrice.Symbol);

            if (currentPrice is null)
            {
                _currentPrices.Add(lastPrice);
                return;
            }

            var lastP = decimal.Parse(lastPrice.LastPrice);
            var currentP = decimal.Parse(currentPrice.LastPrice);

            var rateVariation = (currentP / lastP - 1) * 100;

            var notified = (Math.Abs(rateVariation) >= threshold);
            var isUpOrDown = (rateVariation >= 0) ? IsUpOrDown.Up : IsUpOrDown.Down;

            if (notified)
            {
                Console.ForegroundColor = (isUpOrDown == IsUpOrDown.Up) ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"Symbol: {lastPrice.Symbol} / Current Price: {currentP} / Last Price: {lastP} / Threshold: {threshold:N4}% / Variação: {rateVariation:N4}%");
                Console.ForegroundColor = ConsoleColor.White;

                await SendNotification(new SymbolPriceUpOrDownResult()
                {
                    LastPrice = lastPrice, 
                    IsUpOrDown = isUpOrDown,
                    Threshold = threshold,
                    RateVariation = rateVariation
                });
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"Symbol: {lastPrice.Symbol} / Current Price: {currentP} / Last Price: {lastP} / Threshold: {threshold:N4}% / Variação: {rateVariation:N4}%");
                Console.ForegroundColor = ConsoleColor.White;
            }

            currentPrice.LastPrice = lastPrice.LastPrice;
        }

        private async Task SendNotification(SymbolPriceUpOrDownResult symbolPriceUpOrDownResult)
        {
            var message = System.Text.Json.JsonSerializer.Serialize(symbolPriceUpOrDownResult);
            
            await _publisher.PublishAsync("SymbolPriceUpOrDownResult", message);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"##########Symbol published on observer: {symbolPriceUpOrDownResult.LastPrice.Symbol}");
        }
    }
}

