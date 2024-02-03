using CryptoWatch.Integration.Binance.ApiRest.Domain;
using CryptoWatch.Repository.Interfaces;
using MongoDB.Driver;

namespace CryptoWatch.Repository;

public class ThresholdLogRepository : IThresholdLogRepository
{
    public void Register(SymbolPrice symbolPrice)
    {
        try
        {
            const string connectionUri = "mongodb://guest:guest@localhost:27017";

            var client = new MongoClient(connectionUri);

            var thresholdLogDb = client.GetDatabase("ThresholdLogDb");

            // SymbolPrice
            var symbolPriceCollection = thresholdLogDb.GetCollection<SymbolPrice>("SymbolPrice");

            symbolPriceCollection.InsertOne(symbolPrice);
            
            //Buy or Sell
            var buyOrSellSymbolCollection = thresholdLogDb.GetCollection<Object>("BuyOrSellSymbol");

            buyOrSellSymbolCollection.InsertOne(new
            {
                symbolPrice.Symbol,
                symbolPrice.LastPrice,
                CreatedDate = DateTime.UtcNow,
                BuyOrSell = "buy"
            });

            // Print message log
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Register Message -> MongoDB: {0}", symbolPrice.Symbol);
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}