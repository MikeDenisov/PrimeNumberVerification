namespace PrimeNumber.Client.RateLimiter
{
    public interface IRateLimiter
    {
        void Start();
        Task WaitForSlot(CancellationToken stoppingToken);
    }
}
