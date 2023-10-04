using Grpc.Core;

using PrimeNumberVerificationService.Protos;

namespace PrimeNumberVerificationService.Services
{
    public class PrimeNumberVerificatorService : PrimeNumberVerificator.PrimeNumberVerificatorBase
    {
        private readonly ILogger<PrimeNumberVerificatorService> _logger;
        public PrimeNumberVerificatorService(ILogger<PrimeNumberVerificatorService> logger)
        {
            _logger = logger;
        }

        public override Task<PrimeNumberReply> ValidateNumber(PrimeNumberRequest request, ServerCallContext context)
        {
            return Task.FromResult(new PrimeNumberReply
            {
                Valid = true
            });
        }
    }
}
