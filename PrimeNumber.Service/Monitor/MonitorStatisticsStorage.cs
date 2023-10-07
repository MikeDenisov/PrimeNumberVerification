using System.Collections.Concurrent;

namespace PrimeNumber.Service.Monitor
{
    public interface IMonitorStatisticsStorage
    {
        long RequestsCount { get; }
        IDictionary<long, long> ValidatedNumbers { get; }
        void IncreaseRequestsCount(long count = 1);
        void AddValidatedNumber(long num, long count = 1);
    }

    /// <summary>
    /// Thread-safe statistics storage
    /// </summary>
    public class MonitorStatisticsStorage: IMonitorStatisticsStorage
    {
        private long _requestsCounter;
        private readonly ConcurrentDictionary<long, long> _validatedNumbers = new ConcurrentDictionary<long, long>();

        public long RequestsCount => _requestsCounter;
        public IDictionary<long, long> ValidatedNumbers => _validatedNumbers;

        public void IncreaseRequestsCount(long count = 1)
        {
            Interlocked.Add(ref _requestsCounter, count);
        }

        public void AddValidatedNumber(long num, long count = 1)
        {
            _validatedNumbers.AddOrUpdate(num, count, (key, val) => val + count);
        }
    }
}
