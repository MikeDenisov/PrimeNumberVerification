using Grpc.Net.Client;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using PowerArgs;
using PrimeNumber.Client.Config;
using PrimeNumber.Client.RateLimiter;
using PrimeNumber.Client.Services;
using PrimeNumber.Client.Storage;
using PrimeNumber.Client.Workers;
using PrimeNumber.Shared;

var config = Args.Parse<ClientConfig>(args);

if (config == null) // Wrong args or help
    return;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddSingleton<IClientConfig>(config);
builder.Services.AddTransient<IRateLimiter, FixedWindowRateLimiter>(services => new FixedWindowRateLimiter(config.RequestsPerSecond));
builder.Services.AddSingleton<IExecutionStorage, ExecutionStorage>();
builder.Services.AddSingleton(GrpcChannel.ForAddress(config.RemoteAddress));
builder.Services.AddHostedService<RequestExecutionServie>();
builder.Services.AddHostedService<ResponsesProcessingService>();
builder.Services.AddSingleton<IPrimeNumberValidator>(new PrimeNumberValidator(config.NumberLimit));

var host = builder.Build();

await host.StartAsync();

Console.ReadLine();