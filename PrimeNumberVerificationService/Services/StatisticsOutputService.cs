namespace PrimeNumberVerificationService.Services
{
    public class StatisticsOutputService: BackgroundService
    {
        private const int Delay = 1000;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                PrintStatisctic();
                await Task.Delay(Delay);
            }
        }

        private void PrintStatisctic()
        {
            Console.Clear();
            Console.WriteLine("Service stats:");
            Console.WriteLine($"Current time: {DateTime.Now}");
        }
    }
}
