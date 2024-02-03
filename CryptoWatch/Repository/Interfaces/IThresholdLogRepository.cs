using CryptoWatch.Integration.Binance.ApiRest.Domain;

namespace CryptoWatch.Repository.Interfaces;

public interface IThresholdLogRepository
{
    public void Register(SymbolPrice symbolPrice);
}