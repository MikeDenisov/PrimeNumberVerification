using PrimeNumber.Service.Config;
using PrimeNumber.Service.Monitor.Output;

namespace PrimeNumber.Service.Monitor
{
    public class MonitorStatisticsProcessor: BackgroundService
    {
        private readonly IMonitorStatisticsStorage _statisticsStorage;
        private readonly IMonitorOutputStrategy _outputStrategy;
        private readonly TimeSpan _delay;
        private long _previousCount = 0;

        public MonitorStatisticsProcessor(IMonitorStatisticsStorage statisticsStorage, IMonitorOutputStrategy outputStrategy, ServiceConfig configuration)
        {
            _statisticsStorage = statisticsStorage;
            _outputStrategy = outputStrategy;
            _delay = TimeSpan.FromSeconds(configuration.OutputDelaySec);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ExecuteStrategy();
                await Task.Delay(_delay);
            }
        }

        private Task ExecuteStrategy()
        {
            var topNumbers = _statisticsStorage.ValidatedNumbers
                .ToArray()
                .OrderByDescending(entry => entry.Value)
                .Take(10)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            var stats = new ServiceStatsModel()
            {
                TimeStamp = DateTime.UtcNow,
                RequestsTotalCount = _statisticsStorage.RequestsCount,
                RequestsPerSecond = (_statisticsStorage.RequestsCount - _previousCount) / _delay.TotalSeconds,
                TopRequestedNumbers = topNumbers
            };

            _previousCount = _statisticsStorage.RequestsCount;

            return _outputStrategy.Execute(stats);
        }
    }
}
