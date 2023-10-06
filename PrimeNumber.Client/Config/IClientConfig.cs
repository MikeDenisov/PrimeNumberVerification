namespace PrimeNumber.Client.Config
{
    public interface IClientConfig
    {
        int ExecutionTime { get; }
        long NumberLimit { get; }
        string RemoteAddress { get; }
        int RequestsPerSecond { get; }
    }
}