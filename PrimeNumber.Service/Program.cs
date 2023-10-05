using Calzolari.Grpc.AspNetCore.Validation;

using PrimeNumber.Service.Config;
using PrimeNumber.Service.MessageBus;
using PrimeNumber.Service.Monitor;
using PrimeNumber.Service.Validation;
using PrimeNumber.Shared;

using PrimeNumberVerificationService.Services;

using SlimMessageBus.Host;
using SlimMessageBus.Host.Memory;

// DI Configuration ->
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options => options.EnableMessageValidation());

// Validation
builder.Services.AddGrpcValidation();
builder.Services.AddValidator<PrimeNumberRequestValidator>();

// Monitoring
builder.Services.AddHostedService<ConsoleStatisticsPrinter>();
builder.Services.AddSingleton<IMonitorStatisticsStorage, MonitorStatisticsStorage>();

// Validation algorithm
builder.Services.AddSingleton<IPrimeNumberValidator>(provider =>
{
    var limit = provider.GetService<IConfiguration>()?.GetValue<long>(ConfigurationKeys.PrimeNumberLimit);
    return new PrimeNumberValidator(limit ?? 1000);
});

// Message Bus
builder.Services.AddSlimMessageBus(mbb =>
{
    mbb
    .Produce<PrimeNumberRequestMessage>(x => x.DefaultTopic(MessageBusKeys.Topics.Local))
    .Consume<PrimeNumberRequestMessage>(x => x.Topic(MessageBusKeys.Topics.Local).WithConsumer<PrimeNumberRequestMessageConsumer>())
    .WithProviderMemory(settings =>
    {
        settings.EnableMessageSerialization = false;
    });

    mbb.AddServicesFromAssemblyContaining<PrimeNumberRequestMessage>();
});
// DI Configuration <-

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<PrimeNumberVerificatorService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

await app.RunAsync();
