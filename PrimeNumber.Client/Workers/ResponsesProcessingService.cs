using Microsoft.Extensions.Hosting;
using PrimeNumber.Client.Model;
using PrimeNumber.Client.Storage;
using PrimeNumber.Shared;

using System.Collections.Concurrent;

namespace PrimeNumber.Client.Workers
{
    public class ResponsesProcessingService : BackgroundService
    {
        private readonly IPrimeNumberValidator _validator;
        private readonly ConcurrentQueue<RequestResultModel> _requestsQueue;
        private readonly ExecutionStatistics _statsStorage;

        public ResponsesProcessingService(IPrimeNumberValidator validator, IExecutionStorage executionStorage)
        {
            _validator = validator;
            _requestsQueue = executionStorage.CompletedRequests;
            _statsStorage = executionStorage.Statistics;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!_requestsQueue.TryDequeue(out var request))
                {
                    await Task.Yield();
                    continue;
                }
                ProcessRequestResult(request);
            }
        }

        private void ProcessRequestResult(RequestResultModel request)
        {
            var isValid = request.IsSuccessfull && request.RemoteResult == _validator.IsValid(request.Number);
            _statsStorage.AverageRTT = (_statsStorage.AverageRTT * _statsStorage.TotalRequests + request.RoundTripTime.Ticks) / (_statsStorage.TotalRequests + 1);
            _statsStorage.TotalRequests++;
            _statsStorage.FailedRequests += request.IsSuccessfull ? 0 : 1;

            PrintToConsole(request, isValid);
        }

        private void PrintToConsole(RequestResultModel request, bool isValid)
        {
            var initialColor = Console.ForegroundColor;

            if (!isValid)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.Write($"#{request.Id,6} :: N {request.Number,6} :: RTT {request.RoundTripTime.TotalMilliseconds,6} ms :: Prime: {request.RemoteResult,5} :: ");
            Console.ForegroundColor = isValid ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(isValid ? "Valid" : "Invalid");
            Console.ForegroundColor = initialColor;
        }
    }
}
