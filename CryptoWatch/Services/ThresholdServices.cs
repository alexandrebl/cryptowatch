using CryptoWatch.Integration.Binance.ApiRest.Domain;
using CryptoWatch.Services.Interfaces;
using CryptoWatch.Worker;
using StackExchange.Redis;
using System.Collections.Generic;

namespace CryptoWatch.Services
{
    public class ThresholdServices : IThresholdServices
    {
        private readonly ILogger<ThresholdWorker> _logger;
        private readonly IDatabase _redisDatabaseLastPrices;
        private IList<SymbolPrice> _currentPrices;

        public ThresholdServices(ILogger<ThresholdWorker> logger, [FromKeyedServices("LastPrices")] IDatabase redisDatabaseLastPrices)
        {
            _logger = logger;
            _redisDatabaseLastPrices = redisDatabaseLastPrices;
            _currentPrices = new List<SymbolPrice>();
        }

        // todo Last -> 0.1% -> Current com o ultimo valor

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var lastPrices = _redisDatabaseLastPrices.HashGetAll("LastPrices").
                                Select(s => new SymbolPrice() { Symbol = s.Name, LastPrice = s.Value });

            foreach (var lastPrice in lastPrices)
            { 
                await GetUpDown(lastPrice);
            }

            // todo Limpar variavel current price para moedas nao listadas em LastPrice

            await Task.CompletedTask;
        }


        // 100,00 - Current
        // 120,00 - LastPrice
        // 20,00 - Variacao
        // 20% - Taxa de variacao
        // UP - Verde 

        // 1% - Threshold

        // 100,00
        // 80,00
        // 20,00
        // 20%
        // DW - Vermelho

        public async Task GetUpDown(SymbolPrice lastPrice)
        {
            var threshold = 0.0001M;
            var currentPrice = _currentPrices.FirstOrDefault(f => f.Symbol == lastPrice.Symbol);

            if (currentPrice is null)
            {
                _currentPrices.Add(lastPrice);
                return;
            }

            var lastP = decimal.Parse(lastPrice.LastPrice);
            var currentP = decimal.Parse(currentPrice.LastPrice);

            var taxaVariacao = (currentP / lastP - 1) * 100;

            var notified = (Math.Abs(taxaVariacao) >= threshold);
            var isUpOrDownResult = taxaVariacao >= 0;

            if (notified)
            {
                Console.ForegroundColor = (isUpOrDownResult) ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"Symbol: {lastPrice.Symbol} / Current Price: {currentP} / Last Price: {lastP} / Threshold: {threshold:N4}% / Variação: {taxaVariacao:N4}%");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"Symbol: {lastPrice.Symbol} / Current Price: {currentP} / Last Price: {lastP} / Threshold: {threshold:N4}% / Variação: {taxaVariacao:N4}%");
                Console.ForegroundColor = ConsoleColor.White;
            }

            currentPrice.LastPrice = lastPrice.LastPrice;
        }
    }
}

