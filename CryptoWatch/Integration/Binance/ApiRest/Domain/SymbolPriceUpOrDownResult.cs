using System.Text.Json.Serialization;
using CryptoWatch.Integration.Binance.ApiRest.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CryptoWatch.Integration.Binance.ApiRest.Domain;

[BsonIgnoreExtraElements]
public class SymbolPriceUpOrDownResult
{
    public SymbolPrice LastPrice { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public IsUpOrDown IsUpOrDown { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public decimal Threshold { get; set; }
    public decimal RateVariation { get; set; }
}

