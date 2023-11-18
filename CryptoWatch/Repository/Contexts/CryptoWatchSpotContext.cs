using Microsoft.EntityFrameworkCore;

namespace CryptoWatch.Repository.Domain;

public class CryptoWatchSpotContext : DbContext
{
    public DbSet<SymbolWatch> SymbolWatches { get; set; }
    
    public CryptoWatchSpotContext(DbContextOptions<CryptoWatchSpotContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SymbolWatch>().ToTable("SymbolWatch");

        base.OnModelCreating(modelBuilder);
    }
}