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

            var redisConnection = ConnectionMultiplexer.Connect( 
                Environment.GetEnvironmentVariable("CRYPTOWATCHSPOTCACHECONNECTION"));
            
            services.AddSingleton<IConnectionMultiplexer>(redisConnection);
            // todo O indice do banco deveria vir por configuração
            services.AddKeyedSingleton("LastPrices", redisConnection.GetDatabase(0));

            #endregion
                        
            #region Entity Framework
            services.AddDbContext<CryptoWatchSpotContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("CRYPTOWATCHSPOTCONNECTION")), ServiceLifetime.Singleton);

            #endregion

            #region Dependecy Injection
            services.AddSingleton<ITickerPriceIntegration, TickerPriceIntegration>();
            services.AddSingleton<ISymbolRepository, SymbolRepository>();
            services.AddSingleton<ITickerPriceServices, TickerPriceServices>();
            services.AddSingleton<IThresholdServices, ThresholdServices>();            
            #endregion

            #region Workers
            services.AddHostedService<ThresholdWorker>();
            services.AddHostedService<TickerWorker>();
            #endregion
        })
    .Build();

host.Run();