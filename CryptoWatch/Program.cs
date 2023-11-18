using CryptoWatch.Worker;
using CryptoWatch.Integration.Binance.ApiRest;
using CryptoWatch.Integration.Binance.ApiRest.Interfaces;
using CryptoWatch.Repository;
using CryptoWatch.Repository.Domain;
using CryptoWatch.Repository.Interfaces;
using CryptoWatch.Services;
using CryptoWatch.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(
        services =>
        {
            #region Middlewares
            services.AddHttpClient();
            #endregion
            
            #region Entity Framework
            services.AddDbContext<CryptoWatchSpotContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("CRYPTOWATCHSPOTCONNECTION")));

            #endregion

            #region Dependecy Injection
            services.AddScoped<ITickerPriceIntegration, TickerPriceIntegration>();
            services.AddScoped<ISymbolRepository, SymbolRepository>();
            services.AddScoped<ISymbolWatchServices, SymbolWatchServices>();
            #endregion
            
            #region Workers
            services.AddHostedService<InfoWorker>();
            services.AddHostedService<TickerWorker>();
            #endregion
        })
    .Build();

host.Run();