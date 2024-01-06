using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CryptoWatch.Repository.Domain;

public class CryptoWatchSpotContext : DbContext
{
    public DbSet<SymbolWatch> SymbolWatches { get; set; }
    
    public CryptoWatchSpotContext(DbContextOptions<CryptoWatchSpotContext> options) : base(options)
    {
        //Database.EnsureCreated(); 
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
            //var connectionString = Environment.GetEnvironmentVariable("CRYPTOWATCHSPOTCONNECTION");
            var connectionString =  "Server=localhost,1433;Initial Catalog=CryptoWatchSpot;Persist Security Info=False;User ID=sa;Password=CryptoW@tch2024;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;Connection Timeout=30;";
            
            var optionsBuilder = new DbContextOptionsBuilder<CryptoWatchSpotContext>();
            
            Console.WriteLine("1---------");
            Console.WriteLine(connectionString);
            Console.Write("2---------");
    
           
            optionsBuilder.UseSqlServer(connectionString);
    
            return new CryptoWatchSpotContext(optionsBuilder.Options);
        }
    }
}