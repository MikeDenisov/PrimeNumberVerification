namespace PrimeNumber.Service.Monitor.Output
{
    public class ConsoleMonitorOutputStrategy: IMonitorOutputStrategy
    {
        public Task Execute(ServiceStatsModel stats)
        {
            Console.WriteLine($"UTC Time: {stats.TimeStamp}");
            Console.WriteLine($"Requests per second: {stats.RequestsPerSecond}");
            Console.WriteLine($"Requests Count: {stats.RequestsTotalCount}");

            Console.WriteLine("Most requested prime numbers:");

            foreach (var pair in stats.TopRequestedNumbers)
            {
                Console.WriteLine($"{pair.Key} : {pair.Value}");
            }

            Console.WriteLine();

            return Task.CompletedTask;
        }
    }
}
