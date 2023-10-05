using FluentValidation;

using PrimeNumber.Service.Config;

using PrimeNumberVerificationService.Protos;

namespace PrimeNumber.Service.Validation
{
    public class PrimeNumberRequestValidator: AbstractValidator<PrimeNumberRequest>
    {
        public PrimeNumberRequestValidator(IConfiguration configuration)
        {
            var limit = configuration.GetValue<long>(ConfigurationKeys.PrimeNumberLimit);
            RuleFor(request => request.Id).GreaterThan(0).WithMessage("Id is mandatory.");
            RuleFor(request => request.Timestamp).GreaterThan(0).WithMessage("Timestamp is mandatory.");
            RuleFor(request => request.Number).GreaterThan(0).WithMessage("Number must be positive.");
            RuleFor(request => request.Number).LessThan(limit).WithMessage($"Number must be in limit {limit}.");
        }
    }
}
