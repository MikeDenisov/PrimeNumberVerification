using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimeNumber.Client.Model;

namespace PrimeNumber.Client.Storage
{
    public interface IExecutionStorage
    {
        ConcurrentQueue<RequestResultModel> CompletedRequests { get; }

        ExecutionStatistics Statistics { get; }
    }

    public class ExecutionStorage : IExecutionStorage
    {
        private readonly ConcurrentQueue<RequestResultModel> _completedRequests = new ConcurrentQueue<RequestResultModel>();

        public ConcurrentQueue<RequestResultModel> CompletedRequests => _completedRequests;

        public ExecutionStatistics Statistics { get; } = new ExecutionStatistics();
    }
}
