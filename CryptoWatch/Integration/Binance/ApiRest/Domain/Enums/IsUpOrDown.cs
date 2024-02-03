using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CryptoWatch.Integration.Binance.ApiRest.Domain.Enums;


public enum IsUpOrDown
{
    Down = 1,
    Up = 10
}