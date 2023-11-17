using CryptoWatch.Worker;
using CryptoWatch.Integration.Binance.ApiRest;
using CryptoWatch.Integration.Binance.ApiRest.Interfaces;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(
        services =>
        {
            #region Middlewares
            services.AddHttpClient();
            #endregion

            #region Dependecy Injection
            services.AddSingleton<ITickerPriceIntegration, TickerPriceIntegration>();
            #endregion
            
            #region Workers
            services.AddHostedService<InfoWorker>();
            services.AddHostedService<TickerWorker>();
            #endregion
        })
    .Build();

host.Run();