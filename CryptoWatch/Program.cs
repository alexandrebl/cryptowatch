using CryptoWatch;
using CryptoWatch.Worker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(
        services =>
        {
            services.AddHostedService<InfoWorker>();
        })
    .Build();

host.Run();