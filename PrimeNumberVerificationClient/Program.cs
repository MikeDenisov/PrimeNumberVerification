using Grpc.Net.Client;
using PrimeNumberVerificationService.Protos;

using static PrimeNumberVerificationService.Protos.PrimeNumberVerificator;

using var channel = GrpcChannel.ForAddress("https://localhost:7091");
var client = new PrimeNumberVerificatorClient(channel);

var reply = await client.ValidateNumberAsync(
                  new PrimeNumberRequest() { Id = 1, Number = 3, Timestamp = DateTime.UnixEpoch.Ticks });

Console.WriteLine("Result: " + reply.Valid);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
