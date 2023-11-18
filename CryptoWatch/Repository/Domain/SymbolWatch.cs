using Microsoft.VisualBasic;

namespace CryptoWatch.Repository.Domain;

public class SymbolWatch
{
    public long Id { get; set; }
    public string Symbol { get; set; }

    public SymbolWatch()
    {
        this.Symbol = string.Empty;
    }
}