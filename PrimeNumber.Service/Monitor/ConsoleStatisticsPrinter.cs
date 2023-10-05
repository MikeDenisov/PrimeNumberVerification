using PrimeNumber.Service.Config;

namespace PrimeNumber.Service.Monitor
{
    public class ConsoleStatisticsPrinter: BackgroundService
    {
        private readonly IMonitorStatisticsStorage _statisticsStorage;
        private readonly int _delay;

        public ConsoleStatisticsPrinter(IMonitorStatisticsStorage statisticsStorage, IConfiguration configuration)
        {
            _statisticsStorage = statisticsStorage;
            _delay = configuration.GetValue<int>(ConfigurationKeys.ConsoleOutputDelay);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                PrintStatsToConsole();
                await Task.Delay(_delay);
            }
        }

        private void PrintStatsToConsole()
        {
            Console.Clear();
            Console.WriteLine("Service stats:");
            Console.WriteLine($"Current time: {DateTime.Now}");

            var topNumbers = _statisticsStorage.ValidatedNumbers
                .OrderByDescending(entry => entry.Value)
                .Take(10)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            Console.WriteLine($"Requests Count: {_statisticsStorage.RequestsCount}");

            Console.WriteLine("Most requested prime numbers:");
            foreach (var pair in topNumbers)
            {
                Console.WriteLine($"{pair.Key} : {pair.Value}");
            }
        }
    }
}
