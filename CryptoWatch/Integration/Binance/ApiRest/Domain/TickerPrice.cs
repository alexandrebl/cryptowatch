using System.Security.Principal;

namespace CryptoWatch.Integration.Binance.ApiRest.Domain;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class TickerPrice
{
    public DateTime UpdatedAt { get; init; }
    public string Symbol { get; set; } = null!;
    public string PriceChange { get; set; } = null!;
    public string PriceChangePercent { get; set; } = null!;
    public string WeightedAvgPrice { get; set; } = null!;
    public string OpenPrice { get; set; } = null!;
    public string HighPrice { get; set; } = null!;
    public string LowPrice { get; set; } = null!;
    public string LastPrice { get; set; } = null!;
    public string Volume { get; set; } = null!;
    public string QuoteVolume { get; set; } = null!;
    public long OpenTime { get; set; }
    public long CloseTime { get; set; }
    public long FirstId { get; set; }
    public long LastId { get; set; }
    public int Count { get; set; }

    public TickerPrice()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}