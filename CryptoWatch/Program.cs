using System.Runtime.CompilerServices;
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
using Microsoft.Extensions.Configuration;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(
        services =>
        {
            #region Middlewares
            services.AddHttpClient();

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var redisConnectionString = configuration.GetSection("ConnectionStrings")["Redis"];

            var redisConnection = ConnectionMultiplexer.Connect(redisConnectionString);
            
            services.AddSingleton<IConnectionMultiplexer>(redisConnection);
            // todo O indice do banco deveria vir por configuração
            services.AddKeyedSingleton("LastPrices", redisConnection.GetDatabase(0));

            #endregion
                        
            #region Entity Framework
            
            var connectionString =  configuration.GetSection("ConnectionStrings")["SqlServer"];
            services.AddDbContext<CryptoWatchSpotContext>(
                options =>
                {
                    options.UseSqlServer(connectionString);
                }, ServiceLifetime.Singleton);

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