using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

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
    
    public class CryptoWatchSpotContextFactory : IDesignTimeDbContextFactory<CryptoWatchSpotContext>
    {
        public CryptoWatchSpotContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var sqlServerConnectionString = configuration.GetSection("ConnectionStrings")["SqlServer"];
            
            var optionsBuilder = new DbContextOptionsBuilder<CryptoWatchSpotContext>();
            
            optionsBuilder.UseSqlServer(sqlServerConnectionString);
    
            return new CryptoWatchSpotContext(optionsBuilder.Options);
        }
    }
}