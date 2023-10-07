using PrimeNumber.Service.MessageBus;

using SlimMessageBus;

namespace PrimeNumber.Service.Monitor
{
    public class PrimeNumberRequestMessageHandler: IConsumer<PrimeNumberRequestMessage>
    {
        private readonly IMonitorStatisticsStorage _statisticsStorage;

        public PrimeNumberRequestMessageHandler(IMonitorStatisticsStorage statisticsStorage)
        {
            _statisticsStorage = statisticsStorage;
        }

        public Task OnHandle(PrimeNumberRequestMessage message)
        {
            _statisticsStorage.IncreaseRequestsCount();

            if (message.IsValid)
            {
                _statisticsStorage.AddValidatedNumber(message.Number);
            }

            return Task.CompletedTask;
        }
    }
}
