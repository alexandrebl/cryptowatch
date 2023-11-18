using CryptoWatch.Repository.Domain;
using CryptoWatch.Repository.Interfaces;

namespace CryptoWatch.Repository;

public class SymbolRepository : ISymbolRepository
{

    private readonly CryptoWatchSpotContext _cryptoWatchSpotContext;

    public SymbolRepository(CryptoWatchSpotContext cryptoWatchSpotContext)
    {
        _cryptoWatchSpotContext = cryptoWatchSpotContext;
    }
    public async Task<IEnumerable<SymbolWatch>> GetSymbolsToWatchASync()
    {
        var symbolWatchItems =  _cryptoWatchSpotContext.SymbolWatches.AsEnumerable();

        await Task.Delay(1);
        return symbolWatchItems;
    }
}