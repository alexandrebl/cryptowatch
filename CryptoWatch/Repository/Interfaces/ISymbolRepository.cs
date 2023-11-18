using CryptoWatch.Repository.Domain;

namespace CryptoWatch.Repository.Interfaces;

public interface ISymbolRepository
{
    public Task<IEnumerable<SymbolWatch>> GetSymbolsToWatchASync();
}