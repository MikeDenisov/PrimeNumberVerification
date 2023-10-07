namespace PrimeNumber.Service.Monitor.Output
{
    public interface IMonitorOutputStrategy
    {
        Task Execute(ServiceStatsModel stats);
    }
}
