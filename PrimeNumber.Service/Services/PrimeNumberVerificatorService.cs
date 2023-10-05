using Grpc.Core;

using PrimeNumber.Service.MessageBus;
using PrimeNumber.Shared;

using PrimeNumberVerificationService.Protos;

using SlimMessageBus;

namespace PrimeNumberVerificationService.Services
{
    public class PrimeNumberVerificatorService: PrimeNumberVerificator.PrimeNumberVerificatorBase
    {
        private readonly IPrimeNumberValidator _validator;
        private readonly IMessageBus _messageBus;

        public PrimeNumberVerificatorService(IPrimeNumberValidator validator, IMessageBus messageBus)
        {
            _validator = validator;
            _messageBus = messageBus;
        }

        public override Task<PrimeNumberReply> ValidateNumber(PrimeNumberRequest request, ServerCallContext context)
        {
            var isValid = _validator.IsValid(request.Number);

            // Using an interceptor to trigger MB might look more elegant, especially when more endpoints are introduced.
            _messageBus.Publish(new PrimeNumberRequestMessage() { Number = request.Number, IsValid = isValid }, cancellationToken: context.CancellationToken).ConfigureAwait(false);

            return Task.FromResult(new PrimeNumberReply
            {
                Valid = isValid
            });
        }
    }
}
