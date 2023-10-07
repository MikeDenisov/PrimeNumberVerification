namespace PrimeNumber.Client.Config
{
    public interface IClientConfig
    {
        int ExecutionTime { get; }
        long NumberLimit { get; }
        string Url { get; }
        int RequestsPerSecond { get; }
    }
}