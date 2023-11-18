namespace CryptoWatch.Integration.Binance.ApiRest.Domain;

public sealed class SymbolPrice
{
    public string Symbol { get; set; }
    public string LastPrice { get; set; }
}