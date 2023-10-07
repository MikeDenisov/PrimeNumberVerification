using Calzolari.Grpc.AspNetCore.Validation;

using PrimeNumber.Service.Config;
using PrimeNumber.Service.MessageBus;
using PrimeNumber.Service.Monitor;
using PrimeNumber.Service.Monitor.Output;
using PrimeNumber.Service.Validation;
using PrimeNumber.Shared;

using PrimeNumberVerificationService.Services;

using SlimMessageBus.Host;
using SlimMessageBus.Host.Memory;

class Program
{
    /// <summary>
    /// Start Prime Number verification web service
    /// </summary>
    /// <param name="urls">A semi-colon delimeted list of URL(s) to configure for the web server</param>
    /// <param name="limit">Max supported input number. May infulence startup time with large values</param>
    /// <param name="outputDelay">Console output refresh rate in seconds</param>
    static async Task Main(string urls = "http://localhost:4242", long limit = 1000, double outputDelay = 1)
    {
        // DI Configuration ->
        var builder = WebApplication.CreateBuilder();

        builder.WebHost
            .UseUrls(urls);

        builder.Services.AddGrpc(options => options.EnableMessageValidation());
        builder.Services.AddSingleton(new ServiceConfig(limit, outputDelay));

        // Validation
        builder.Services.AddGrpcValidation();
        builder.Services.AddValidator<PrimeNumberRequestValidator>();

        // Monitoring
        builder.Services.AddHostedService<MonitorStatisticsProcessor>();
        builder.Services.AddSingleton<IMonitorStatisticsStorage, MonitorStatisticsStorage>();
        builder.Services.AddTransient<IMonitorOutputStrategy, ConsoleMonitorOutputStrategy>();
        builder.Services.AddSingleton<PrimeNumberRequestMessageHandler>();

        // Validation algorithm
        builder.Services.AddSingleton<IPrimeNumberValidator>(provider =>
        {
            return new PrimeNumberValidator(limit);
        });

        // Message Bus
        builder.Services.AddSlimMessageBus(mbb =>
        {
            mbb
            .Produce<PrimeNumberRequestMessage>(x => x.DefaultTopic(MessageBusKeys.Topics.Local))
            .Consume<PrimeNumberRequestMessage>(x => x
                .Topic(MessageBusKeys.Topics.Local)
                .WithConsumer<PrimeNumberRequestMessageHandler>((consumer, message) => consumer.OnHandle(message)))
            .WithProviderMemory(settings =>
            {
                settings.EnableMessageSerialization = false;
            });
        });
        // DI Configuration <-

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<PrimeNumberVerificatorService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        await app.RunAsync();
    }
}