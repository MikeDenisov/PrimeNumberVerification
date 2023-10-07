using Grpc.Net.Client;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using PrimeNumber.Client.Config;
using PrimeNumber.Client.RateLimiter;
using PrimeNumber.Client.Services;
using PrimeNumber.Client.Storage;
using PrimeNumber.Client.Workers;
using PrimeNumber.Shared;

class Program
{
    /// <summary>
    /// Runs Prime Number web service load test
    /// </summary>
    /// <param name="url">Service url</param>
    /// <param name="time">Test execution time</param>
    /// <param name="rate">Target requests per second rate</param>
    /// <param name="limit">Max supported input number. May infulence startup time with large values</param>
    static async Task Main(string url = "http://localhost:4242", int time = 1, int rate = 10000, long limit = 1000)
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.AddSingleton<IClientConfig>(new ClientConfig()
        {
            Url = url,
            ExecutionTime = time,
            RequestsPerSecond = rate,
            NumberLimit = limit
        });

        builder.Services.AddTransient<IRateLimiter, FixedWindowRateLimiter>(services => new FixedWindowRateLimiter(rate));
        builder.Services.AddSingleton<IExecutionStorage, ExecutionStorage>();
        builder.Services.AddSingleton(GrpcChannel.ForAddress(url));
        builder.Services.AddHostedService<RequestExecutionServie>();
        builder.Services.AddHostedService<ResponsesProcessingService>();
        builder.Services.AddSingleton<IPrimeNumberValidator>(new PrimeNumberValidator(limit));

        var host = builder.Build();

        await host.StartAsync();
        await host.WaitForShutdownAsync();

        Console.WriteLine("Hit Enter to exit");
        Console.ReadLine();
    }
}