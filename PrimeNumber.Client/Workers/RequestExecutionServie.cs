using Grpc.Core;
using Grpc.Net.Client;

using Microsoft.Extensions.Hosting;

using PrimeNumber.Client.Config;
using PrimeNumber.Client.Helpers;
using PrimeNumber.Client.Model;
using PrimeNumber.Client.RateLimiter;
using PrimeNumber.Client.Storage;
using PrimeNumberVerificationService.Protos;

using System.Diagnostics;

using static PrimeNumberVerificationService.Protos.PrimeNumberVerificator;

namespace PrimeNumber.Client.Services
{
    public class RequestExecutionServie: BackgroundService
    {
        private readonly GrpcChannel _channel;
        private readonly IRateLimiter _rateLimiter;
        private readonly IExecutionStorage _executionStorage;
        private readonly long _numberLimit;
        private readonly int _requestsCount;

        public RequestExecutionServie(GrpcChannel channel, IRateLimiter rateLimiter, IExecutionStorage executionStorage, IClientConfig clientConfig)
        {
            _channel = channel;
            _rateLimiter = rateLimiter;
            _executionStorage = executionStorage;
            _numberLimit = clientConfig.NumberLimit;
            _requestsCount = clientConfig.ExecutionTime * clientConfig.RequestsPerSecond;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield(); // Needed to unblock other services execution

            _executionStorage.Statistics.StartTime = DateTime.UtcNow;

            var requestTasks = new List<Task>(_requestsCount);
            var random = new Random();

            _rateLimiter.Start();

            for (var i = 0; i < _requestsCount; i++)
            {
                await _rateLimiter.WaitForSlot(stoppingToken);

                requestTasks.Add(ExecuteSingleRequestAsync(i, random.NextInt64(_numberLimit), stoppingToken));

                if (stoppingToken.IsCancellationRequested)
                { 
                    break; 
                }
            }

            await Task.WhenAll(requestTasks);

            _executionStorage.Statistics.FinishTime = DateTime.UtcNow;
            _executionStorage.Statistics.IsComplete = true;
        }

        private async Task ExecuteSingleRequestAsync(long id, long number, CancellationToken stoppingToken)
        {
            var result = new RequestResultModel()
            {
                Id = id,
                Number = number
            };
            var sw = new Stopwatch();
            try
            {
                var client = new PrimeNumberVerificatorClient(_channel);

                var request = new PrimeNumberRequest()
                {
                    Id = id,
                    Number = number,
                    Timestamp = DateTime.Now.ToUnixTime()
                };
                sw.Start();
                var reply = await client.ValidateNumberAsync(request, cancellationToken: stoppingToken);
                result.RemoteResult = reply.Valid;
                result.Status = Status.DefaultSuccess;
            }
            catch (RpcException ex)
            {
                result.Status = ex.Status;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                sw.Stop();
                result.RoundTripTime = sw.Elapsed;
                _executionStorage.CompletedRequests.Enqueue(result);
            }
        }
    }
}
