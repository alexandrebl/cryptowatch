using CryptoWatch.Worker;
using CryptoWatch.Integration.Binance.ApiRest;
using CryptoWatch.Integration.Binance.ApiRest.Interfaces;
using CryptoWatch.Repository;
using CryptoWatch.Repository.Domain;
using CryptoWatch.Repository.Interfaces;
using CryptoWatch.Services;
using CryptoWatch.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(
        services =>
        {
            #region Middlewares
            services.AddHttpClient();
            
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Environment.GetEnvironmentVariable("CRYPTOWATCHSPOTCACHECONNECTION");
            });
            
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("CRYPTOWATCHSPOTCACHECONNECTION"));
            #endregion

            
            #region Entity Framework
            services.AddDbContext<CryptoWatchSpotContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("CRYPTOWATCHSPOTCONNECTION")), ServiceLifetime.Singleton);

            #endregion

            #region Dependecy Injection
            services.AddSingleton<ITickerPriceIntegration, TickerPriceIntegration>();
            services.AddSingleton<ISymbolRepository, SymbolRepository>();
            services.AddSingleton<ITickerPriceServices, TickerPriceServices>();
            #endregion
            
            #region Workers
            //services.AddHostedService<InfoWorker>();
            services.AddHostedService<TickerWorker>();
            #endregion
        })
    .Build();

host.Run();