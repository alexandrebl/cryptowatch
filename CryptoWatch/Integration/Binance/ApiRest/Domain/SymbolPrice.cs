using MongoDB.Bson.Serialization.Attributes;

namespace CryptoWatch.Integration.Binance.ApiRest.Domain;

using MongoDB.Driver;

[BsonIgnoreExtraElements]
public sealed class SymbolPrice
{
    public string Symbol { get; set; }
    public string LastPrice { get; set; }
}