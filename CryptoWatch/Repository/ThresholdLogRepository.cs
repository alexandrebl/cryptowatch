using CryptoWatch.Integration.Binance.ApiRest.Domain;
using CryptoWatch.Repository.Interfaces;
using MongoDB.Driver;

namespace CryptoWatch.Repository;

public class ThresholdLogRepository : IThresholdLogRepository
{
    public void Register(SymbolPriceUpOrDownResult symbolPriceUpOrDownResult)
    {
        try
        {
            const string connectionUri = "mongodb://guest:guest@localhost:27017";

            var client = new MongoClient(connectionUri);

            var thresholdLogDb = client.GetDatabase("ThresholdLogDb");

            // SymbolPrice
            var symbolPriceCollection = thresholdLogDb.GetCollection<SymbolPrice>("LastPrice");

            symbolPriceCollection.InsertOne(symbolPriceUpOrDownResult.LastPrice);
            
            //Up or Down Last Price
            var symbolPriceUpOrDownResultCollection = thresholdLogDb.GetCollection<SymbolPriceUpOrDownResult>(nameof(SymbolPriceUpOrDownResult));

            symbolPriceUpOrDownResultCollection.InsertOne(symbolPriceUpOrDownResult);
    
            // Print message log
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Register Message -> MongoDB: {0}", symbolPriceUpOrDownResult.LastPrice.Symbol);
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}