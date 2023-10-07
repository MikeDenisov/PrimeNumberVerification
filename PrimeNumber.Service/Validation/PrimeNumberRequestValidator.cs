using FluentValidation;

using PrimeNumber.Service.Config;

using PrimeNumberVerificationService.Protos;

namespace PrimeNumber.Service.Validation
{
    public class PrimeNumberRequestValidator: AbstractValidator<PrimeNumberRequest>
    {
        public PrimeNumberRequestValidator(ServiceConfig configuration)
        {
            RuleFor(request => request.Timestamp).GreaterThan(0).WithMessage("Timestamp is mandatory.");
            RuleFor(request => request.Number).LessThan(configuration.PrimeNumberLimit).WithMessage($"Number must be in limit {configuration.PrimeNumberLimit}.");
        }
    }
}
