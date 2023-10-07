using Grpc.Core;

using Microsoft.Extensions.Hosting;

using PrimeNumber.Client.Config;
using PrimeNumber.Client.Model;
using PrimeNumber.Client.Storage;
using PrimeNumber.Shared;

using System.Collections.Concurrent;

namespace PrimeNumber.Client.Workers
{
    public class ResponsesProcessingService : BackgroundService
    {
        private readonly IClientConfig _clientConfig;
        private readonly IPrimeNumberValidator _validator;
        private readonly ConcurrentQueue<RequestResultModel> _requestsQueue;
        private readonly ExecutionStatistics _statsStorage;
        private readonly IHostApplicationLifetime _lifetime;
        private Dictionary<Status, int> _statusCounts = new Dictionary<Status, int>();

        public ResponsesProcessingService(IClientConfig clientConfig, IPrimeNumberValidator validator, IExecutionStorage executionStorage, IHostApplicationLifetime lifetime)
        {
            _clientConfig = clientConfig;
            _validator = validator;
            _lifetime = lifetime;
            _requestsQueue = executionStorage.CompletedRequests;
            _statsStorage = executionStorage.Statistics;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while ((!_statsStorage.IsComplete || _requestsQueue.Any()) && !stoppingToken.IsCancellationRequested)
            {
                while (_requestsQueue.TryDequeue(out var request))
                {
                    ProcessRequestResult(request);
                }

                await Task.Yield();
            }

            PrintFinalStats();
            _lifetime.StopApplication();
        }

        private void ProcessRequestResult(RequestResultModel request)
        {
            var isValid = request.Status.StatusCode == StatusCode.OK && request.RemoteResult == _validator.IsValid(request.Number);
            _statsStorage.AverageRTT = (_statsStorage.AverageRTT * _statsStorage.TotalRequests + request.RoundTripTime.Ticks) / (_statsStorage.TotalRequests + 1);
            _statsStorage.TotalRequests++;
            _statsStorage.FailedRequests += request.Status.StatusCode == StatusCode.OK ? 0 : 1;
            
            if(!_statusCounts.TryAdd(request.Status, 1))
            {
                _statusCounts[request.Status]++;
            }

            PrintToConsole(request, isValid);
        }

        private void PrintToConsole(RequestResultModel request, bool isValid)
        {
            var statsStr = $"#{request.Id,6} :: N {request.Number,6} :: RTT {request.RoundTripTime.TotalMilliseconds,8} ms :: Prime: {request.RemoteResult,5} :: ";
            ConsoleExt.WriteInColor(request.Status.StatusCode == StatusCode.OK ? Console.ForegroundColor : ConsoleColor.Red, statsStr);
            ConsoleExt.WriteLineInColor(isValid ? ConsoleColor.Green : ConsoleColor.Red, isValid ? "Valid" : "Invalid");
        }

        private void PrintFinalStats()
        {
            Console.WriteLine("Execution Complete");

            Console.WriteLine($"Requests Sent: {_statsStorage.TotalRequests}");

            ConsoleExt.WriteLineInColor(
                (Console.ForegroundColor, "Failed Requests:"), 
                (_statsStorage.FailedRequests == 0 ? ConsoleColor.Green : ConsoleColor.Red, _statsStorage.FailedRequests.ToString()));

            Console.WriteLine($"Average Round Trip Time: {TimeSpan.FromTicks(_statsStorage.AverageRTT).TotalMilliseconds} ms");
            Console.WriteLine($"Time Started: {_statsStorage.StartTime} :: Time Finished: {_statsStorage.FinishTime} :: Execution Time: {_statsStorage.ExecutionTime}");
            Console.WriteLine($"Target Rate: {_clientConfig.RequestsPerSecond}");

            ConsoleExt.WriteLineInColor(
                (Console.ForegroundColor, "Actual Average Rate:"), 
                (_statsStorage.AverateRequestsPerSec == _clientConfig.RequestsPerSecond ? ConsoleColor.Green : ConsoleColor.Red, 
                    _statsStorage.AverateRequestsPerSec.ToString()));

            Console.WriteLine("Requests aggregation:");

            foreach (var pair in _statusCounts)
            {
                ConsoleExt.WriteLineInColor(
                    (pair.Key.StatusCode == StatusCode.OK ? ConsoleColor.Green : ConsoleColor.Red, $"{pair.Key.StatusCode,15}"),
                    (Console.ForegroundColor, $" :: Count: { pair.Value, 7 } ::"),
                    (Console.ForegroundColor, $" :: {pair.Key.Detail} ::"));
            }
        }
    }
}
