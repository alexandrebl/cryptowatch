using CryptoWatch.Integration.Binance.ApiRest.Domain;

namespace CryptoWatch.Integration.Binance.ApiRest.Interfaces;

public interface ITickerPriceIntegration
{
    public IEnumerable<TickerPrice> GetPrices(IEnumerable<string> symbols);
}